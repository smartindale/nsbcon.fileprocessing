

namespace Acme.FileManagement.Endpoint.Handlers.NaiveImpl
{
    using Acme.Parser.Contracts.Events;
    using Acme.Processor.Contracts.Events;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Saga;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class FileStatusSaga : Saga<FileStatusSagaData>
        , IAmStartedByMessages<IParsedAnIncomingFile>
        , IHandleMessages<IProcessedARecordset>

    {
        static ILog _log = LogManager.GetLogger(typeof(FileStatusSaga));
        public FileStatusSaga()
        {

        }

        public override void ConfigureHowToFindSaga()
        {
            base.ConfigureHowToFindSaga();
            base.ConfigureMapping<IParsedAnIncomingFile>(e => e.FileId).ToSaga(s => s.FileId);
            base.ConfigureMapping<IProcessedARecordset>(e => e.FileId).ToSaga(s => s.FileId);
        }

        public void Handle(IParsedAnIncomingFile message)
        {
            _log.WarnFormat("Starting up the saga for a new file with {0} records...", message.TotalLineCount);
            Data.FileId = message.FileId;
            Data.Start = DateTime.Now;
            Data.TotalParsed = message.TotalLineCount;
        }

        public void Handle(IProcessedARecordset message)
        {
            Data.FileId = message.FileId;
            if (DateTime.MinValue == Data.Start)
                Data.Start = DateTime.Now;
            _log.WarnFormat("Got some processed data... {0} errors and {1} processed",
                message.TotalErrored,
                message.TotalSucceeded);
            Data.TotalErrored += message.TotalErrored;
            Data.TotalProcessed += message.TotalSucceeded;
            _log.WarnFormat("Running Total: {0} errors and {1} processed",
                Data.TotalErrored
                , Data.TotalProcessed);
            if(IsComplete())
            {
                MarkAsComplete();
                _log.WarnFormat("*****FINISHED THE SAGA******");
                _log.WarnFormat("Total Time To Run: {0}s", DateTime.Now.Subtract(Data.Start).TotalSeconds);
            }
        }

        private bool IsComplete()
        {
            return Data.TotalProcessed + Data.TotalErrored == Data.TotalParsed;
        }

        
    }
}

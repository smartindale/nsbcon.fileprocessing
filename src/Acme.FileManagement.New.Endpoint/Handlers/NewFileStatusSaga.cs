using Acme.FileManagement.Contracts.Messages;
using Acme.Parser.Contracts.Events;
using NServiceBus.Logging;
using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.FileManagement.New.Endpoint.Handlers
{
    public class NewFileStatusSaga : Saga<NewFileStatusSagaData>
        , IAmStartedByMessages<IParsedAnIncomingFile>
        , IHandleTimeouts<IShouldCheckToSeeIfTheFileIsDone>
    {
        static ILog _log = LogManager.GetLogger(typeof(NewFileStatusSaga));
        public NewFileStatusSaga()
        {

        }

        public override void ConfigureHowToFindSaga()
        {
            base.ConfigureHowToFindSaga();
            base.ConfigureMapping<IParsedAnIncomingFile>(e => e.FileId).ToSaga(s => s.FileId);
        }

        public void Handle(IParsedAnIncomingFile message)
        {
            _log.WarnFormat("Starting up the saga for a new file with {0} records...", message.TotalLineCount);
            Data.FileId = message.FileId;
            Data.Start = DateTime.Now;
            Data.TotalParsed = message.TotalLineCount;
            CheckStatus();
        }

        private int GetCounts(Guid fileId)
        {
            var mySql = String.Format(sql, fileId);
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            using (var cmd = new SqlCommand(mySql,conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (Convert.IsDBNull(result)) return 0;
                return Convert.ToInt32(result);
            }
        }

        private string sql = @"SELECT SUM(ErrorCount + SuccessCount) FROM [dbo].[ProcessedRecordset]
            WHERE FileId = '{0}'";

        public void Timeout(IShouldCheckToSeeIfTheFileIsDone state)
        {
            CheckStatus();
        }

        private void CheckStatus()
        {
            var totalSoFar = GetCounts(Data.FileId);
            if (totalSoFar >= Data.TotalParsed)
            {
                MarkAsComplete();
                _log.WarnFormat("*****FINISHED THE SAGA******");
                _log.WarnFormat("Total Time To Run: {0}s", DateTime.Now.Subtract(Data.Start).TotalSeconds);
            }
            else
            {
                _log.WarnFormat("Running Total: {0} of {1}",
               totalSoFar
               , Data.TotalParsed);
                base.RequestTimeout<IShouldCheckToSeeIfTheFileIsDone>(TimeSpan.FromSeconds(1));
            }

        }
    }
}

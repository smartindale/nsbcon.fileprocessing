using Acme.Processor.Contracts.Commands;
using Acme.Processor.Contracts.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Processor.Endpoint.Handlers
{
    public class RecordsetHandler : IHandleMessages<ProcessARecordset>
    {
        private IBus _bus;
        static ILog _log = LogManager.GetLogger(typeof(RecordsetHandler));
        public RecordsetHandler(IBus bus)
        {
            _bus = bus;
        }
        public void Handle(ProcessARecordset message)
        {
            System.Threading.Thread.Sleep(1000);
            var err = new Random().Next(message.TotalCount);
            var success = message.TotalCount - err;
            _log.InfoFormat("Handled a recordset... {0} errors and {1} success", err, success);
            _bus.Publish<IProcessedARecordset>(e =>
            {
                e.FileId = message.FileId;
                e.TotalErrored = err;
                e.TotalSucceeded = success;
            });
            InsertCounts(message.FileId,success,err);
        }



        #region Step 2
        private void InsertCounts(Guid fileId, int success,int err)
        {
            var mySql = String.Format(sql, fileId, err,success);
            using(var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            using (var cmd = new SqlCommand(mySql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private string sql = @"INSERT INTO [dbo].[ProcessedRecordset]
           ([FileId]
           ,[ErrorCount]
           ,[SuccessCount])
     VALUES
           ('{0}'
           ,{1}
           ,{2})";

        #endregion
    }
}

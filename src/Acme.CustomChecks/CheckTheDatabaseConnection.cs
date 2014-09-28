
namespace Acme.CustomChecks
{
    using Acme.CustomChecks.Helpers;
    using NServiceBus.Logging;
    using ServiceControl.Plugin.CustomChecks;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;

    public abstract class CheckTheDatabaseConnection : CustomCheck
    {
        static ILog _log = LogManager.GetLogger(typeof(CheckTheDatabaseConnection));


        public CheckTheDatabaseConnection(string connectionString, string anyTable)
            :base("CheckingTheDatabaseConnection", "Environmental")
        {
            try
            {
                new SqlConnectionTest(connectionString, anyTable)
                    .Test();
                _log.Info("Successfully ran the Database Connection CustomCheck");
                ReportPass();
            }
            catch(Exception e)
            {
                ReportFailed(e.ToString());
                _log.ErrorFormat("Error running Database Connection CustomCheck: {0}", e.ToString());
            }
        }
    }
}

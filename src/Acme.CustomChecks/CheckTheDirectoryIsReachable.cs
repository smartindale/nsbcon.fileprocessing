using NServiceBus.Logging;
using ServiceControl.Plugin.CustomChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.CustomChecks
{
    public abstract class CheckTheDirectoryIsReachable : CustomCheck
    {
        static ILog _log = LogManager.GetLogger(typeof(CheckTheDirectoryIsReachable));
        public CheckTheDirectoryIsReachable(string directoryToCheck)
            : base("CheckTheDirectoryIsReachable", "Environmental")
        {
            try
            {
                if (!Directory.Exists(directoryToCheck))
                    throw new Exception(String.Format("The directory {0} either does not exist or is not reachable.", directoryToCheck));

                _log.InfoFormat("Successfully ran the Directory Connection CustomCheck for {0}", directoryToCheck);
                ReportPass();
            }
            catch(Exception e)
            {
                ReportFailed(e.ToString());
                _log.ErrorFormat("Error running Directory Connection CustomCheck: {0}", e.ToString());

            }
        }
    }
}

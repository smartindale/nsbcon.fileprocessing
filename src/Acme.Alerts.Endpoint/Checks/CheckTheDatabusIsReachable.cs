using Acme.CustomChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Alerts.Endpoint.Checks
{
    public class CheckTheDatabusIsReachable : 
        CheckTheDirectoryIsReachable
    {
        public CheckTheDatabusIsReachable()
            :base(@"C:\databus")
        {

        }
    }
}

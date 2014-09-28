using NServiceBus;
using NServiceBus.Unicast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.CustomChecks
{
    public class ConsoleTitle : IWantToRunWhenBusStartsAndStops
    {
        private readonly IBus _bus;

        public ConsoleTitle(IBus bus)
        {
            _bus = bus;
        }
        public void Start()
        {
            var unicast = _bus as UnicastBus;

            if (System.Diagnostics.Debugger.IsAttached && null != unicast)
                Console.Title = unicast.InputAddress.Queue;
        }

        public void Stop()
        {
        }
    }
}

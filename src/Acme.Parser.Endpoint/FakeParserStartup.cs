using Acme.Parser.Contracts.Events;
using Acme.Processor.Contracts.Commands;
using Acme.Processor.Contracts.Events;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Parser.Endpoint
{
    public class FakeParserStartup : IWantToRunWhenBusStartsAndStops
    {

        private readonly IBus _bus;
        public FakeParserStartup(IBus bus)
        {
            _bus = bus;
        }
        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Press <ENTER> to simulate a file parse and start the saga...");
                Console.ReadLine();
                var fileId = Guid.NewGuid();
                var totalLines = 2 * 1000 * 10;
                var recordSetSize = 1000;
                _bus.Publish<IParsedAnIncomingFile>(e =>
                {
                    e.FileId = fileId;
                    e.TotalLineCount = totalLines;
                });
                var i = 0;
                while(i < totalLines)
                {
                    _bus.Send<ProcessARecordset>(e =>
                    {
                        e.FileId = fileId;
                        e.TotalCount = recordSetSize;
                    });
                    i = i + recordSetSize;
                }
            }
        }

        public void Stop()
        {
            
        }
    }
}

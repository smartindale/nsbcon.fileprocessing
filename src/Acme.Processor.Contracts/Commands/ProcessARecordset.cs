using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Processor.Contracts.Commands
{
    public class ProcessARecordset
    {
        public int TotalCount { get; set; }

        public Guid FileId { get; set; }
    }
}

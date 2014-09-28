using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Processor.Contracts.Events
{
    public interface IProcessedARecordset
    {
        int TotalErrored { get; set; }

        int TotalSucceeded { get; set; }

        Guid FileId { get; set; }
    }
}

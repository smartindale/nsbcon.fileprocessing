using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Parser.Contracts.Events
{
    public interface IParsedAnIncomingFile
    {
        int TotalLineCount { get; set; }

        Guid FileId { get; set; }
    }
}

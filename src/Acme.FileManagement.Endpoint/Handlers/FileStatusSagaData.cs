

namespace Acme.FileManagement.Endpoint.Handlers.NaiveImpl
{

    using NServiceBus.Saga;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class FileStatusSagaData : IContainSagaData
    {
        public virtual Guid Id
        {
            get;
            set;
        }

        public virtual string OriginalMessageId
        {
            get;
            set;
        }

        public virtual string Originator
        {
            get;
            set;
        }

        public virtual Guid FileId { get; set; }

        public virtual int TotalProcessed { get; set; }

        public virtual int TotalErrored { get; set; }



        public virtual int TotalParsed { get; set; }

        public virtual DateTime Start { get; set; }
    }
}

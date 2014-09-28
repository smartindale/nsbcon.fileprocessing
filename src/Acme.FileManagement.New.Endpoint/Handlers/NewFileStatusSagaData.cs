using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.FileManagement.New.Endpoint.Handlers
{
    public class NewFileStatusSagaData : IContainSagaData
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

        [Unique]
        public virtual Guid FileId { get; set; }


        public virtual int TotalParsed { get; set; }

        public virtual DateTime Start { get; set; }
    }
}

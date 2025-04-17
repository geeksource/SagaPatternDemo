using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class PaymentCompletedEvent
    {
        public Guid OrderId { get; set; }
        public DateTime PaidAt { get; set; }
    }

}

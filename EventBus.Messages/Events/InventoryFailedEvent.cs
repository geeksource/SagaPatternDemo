using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class InventoryFailedEvent
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime FailedAt { get; set; } = DateTime.UtcNow;
    }

}

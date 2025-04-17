using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class InventoryReservedEvent
    {
        public Guid OrderId { get; set; }
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
    }

}

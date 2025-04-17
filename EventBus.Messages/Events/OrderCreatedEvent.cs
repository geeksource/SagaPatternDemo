namespace EventBus.Messages.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
}
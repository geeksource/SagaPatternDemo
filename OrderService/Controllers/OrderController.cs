using EventBus.Messages.Events;
using Microsoft.AspNetCore.Mvc;
using OrderService.RabbitMQ;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly EventBusProducer _producer = new();

        [HttpPost("create")]
        public IActionResult CreateOrder()
        {
            var order = new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(),
                TotalAmount = 150
            };

            _producer.PublishOrderCreated(order);
            return Ok($"Order {order.OrderId} created.");
        }
    }
}

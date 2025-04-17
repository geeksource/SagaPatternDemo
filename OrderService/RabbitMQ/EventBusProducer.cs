using EventBus.Messages.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace OrderService.RabbitMQ
{
    public class EventBusProducer
    {
        public void PublishOrderCreated(OrderCreatedEvent order)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("order_exchange", ExchangeType.Fanout);

            var message = JsonConvert.SerializeObject(order);
            var body = Encoding.UTF8.GetBytes(message); 
            channel.BasicPublish("order_exchange","",null,body);    
        }
    }
}

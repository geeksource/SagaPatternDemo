
using EventBus.Messages.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace InventoryService
{
    public class InventoryConsumer : BackgroundService
    {
        public IConnection _connection { get; set; }
        public IModel _channel { get; set; }

        public InventoryConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("payment_exchange", ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "payment_exchange", routingKey: "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.ToArray());
                var paymentEvent = JsonConvert.DeserializeObject<PaymentCompletedEvent>(message);

                Console.WriteLine($"[InventoryService] Received payment for Order {paymentEvent?.OrderId}");

                // Simulate inventory reservation
                var isAvailable =  CheckInventory();

                if (isAvailable)
                {
                    PublishInventoryReserved(new InventoryReservedEvent
                    {
                        OrderId = paymentEvent.OrderId
                    });
                }
                else
                {
                    PublishInventoryFailed(new InventoryFailedEvent
                    {
                        OrderId = paymentEvent.OrderId,
                        Reason = "Item out of stock"
                    });
                }

                await Task.CompletedTask;
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
        private bool CheckInventory()
        {
            // Simulate a 90% chance of stock being available
            return new Random().Next(0, 10) < 9;
        }
        private void PublishInventoryReserved(InventoryReservedEvent evt)
        {
            _channel.ExchangeDeclare("inventory_exchange", ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evt));
            _channel.BasicPublish("inventory_exchange", "", null, body);

            Console.WriteLine($"[InventoryService] Reserved inventory for Order {evt.OrderId}");
        }

        private void PublishInventoryFailed(InventoryFailedEvent evt)
        {
            _channel.ExchangeDeclare("inventory_failed_exchange", ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evt));
            _channel.BasicPublish("inventory_failed_exchange", "", null, body);

            Console.WriteLine($"[InventoryService] Failed to reserve inventory for Order {evt.OrderId}. Reason: {evt.Reason}");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}

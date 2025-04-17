using EventBus.Messages.Events;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PaymentService
{
    public class PaymentConsumer:BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        public PaymentConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel    =   _connection.CreateModel();

            _channel.ExchangeDeclare("order_exchange", ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, "order_exchange", routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var orderEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);

                Console.WriteLine($"[PaymentService] Received order {orderEvent?.OrderId}. Processing payment...");

                // Simulate payment processing
                await Task.Delay(1000);

                // Now publish PaymentCompleted
                var paymentEvent = new PaymentCompletedEvent { OrderId = orderEvent.OrderId };
                PublishPaymentCompleted(paymentEvent);
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
        private void PublishPaymentCompleted(PaymentCompletedEvent payment)
        {
            _channel.ExchangeDeclare("payment_exchange", ExchangeType.Fanout);

            var message = JsonConvert.SerializeObject(payment);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("payment_exchange", "", null, body);
            Console.WriteLine($"[PaymentService] Payment completed for Order {payment.OrderId}");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}

using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using DeliveryService.Service;

namespace DeliveryService
{
    public class OrderConsumer
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _scopeFactory;
        public OrderConsumer(IServiceScopeFactory scopeFactory)
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _scopeFactory = scopeFactory;
        }
        public void ConsumeOrder()
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("orderQueue", false, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received Order: " + message);

                using var scope = _scopeFactory.CreateScope();
                var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();
                deliveryService.AssignDeliveryPartner(message);
            };
            channel.BasicConsume("orderQueue", true, consumer);
        }
    }
}

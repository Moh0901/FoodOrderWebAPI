using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using DeliveryService.Service;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using DeliveryService.Models;
using System.Threading.Channels;

namespace DeliveryService
{
    public class OrderConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public OrderConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("orderQueue", false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" Received Order JSON: " + message);

                try
                {
                    // Deserialize JSON into an Order object
                    var order = JsonSerializer.Deserialize<Order>(message, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Allows case-insensitive property names
                    });

                    if (order == null)
                    {
                        Console.WriteLine(" Error: Deserialized order is null.");
                        return;
                    }

                    using var scope = _scopeFactory.CreateScope();
                    var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();

                    // Pass the deserialized Order object
                    await deliveryService.AssignDeliveryPartner(order);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($" JSON Deserialization Error: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: "orderQueue", autoAck: true, consumer: consumer);
            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
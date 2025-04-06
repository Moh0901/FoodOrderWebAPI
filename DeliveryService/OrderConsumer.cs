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
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;

namespace DeliveryService
    {
        public class OrderConsumer : BackgroundService
        {
            private readonly IServiceScopeFactory _scopeFactory;
            private readonly ConnectionFactory _factory;

            public OrderConsumer(IServiceScopeFactory scopeFactory)
            {
                _scopeFactory = scopeFactory;
                _factory = new ConnectionFactory() { HostName = "localhost" };
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                using var connection = _factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare("orderQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" Received Order JSON: " + message);

                    try
                    {
                        // Deserialize using Newtonsoft.Json
                        var order = JsonConvert.DeserializeObject<Order>(message);

                        if (order == null)
                        {
                            Console.WriteLine(" Error: Deserialized order is null.");
                            return;
                        }

                        using var scope = _scopeFactory.CreateScope();
                        var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();

                        await deliveryService.AssignDeliveryPartner(order);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($" JSON Deserialization Error: {ex.Message}");
                    }
                };

                channel.BasicConsume(queue: "orderQueue", autoAck: true, consumer: consumer);

                Console.WriteLine(" Listening to RabbitMQ queue: orderQueue...");

                await Task.CompletedTask;
            }

            public override void Dispose()
            {
                base.Dispose();
            }
        }
    }
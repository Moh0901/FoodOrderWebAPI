using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using DeliveryService.Service;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService
{
    public class OrderConsumer : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _scopeFactory;
        private IModel _channel;
        private IConnection _connection;

        public OrderConsumer(IServiceScopeFactory scopeFactory)
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _scopeFactory = scopeFactory;
        }

        // Override ExecuteAsync method from BackgroundService
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Ensure that the ConsumeOrder method will respect cancellation
            while (!stoppingToken.IsCancellationRequested)
            {
                await ConsumeOrder(stoppingToken);
                await Task.Delay(5000, stoppingToken); // Delay to avoid constant reconnection attempts, can adjust as needed
            }
        }

        public async Task ConsumeOrder(CancellationToken cancellationToken)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _factory.CreateConnection();
            }

            if (_channel == null || !_channel.IsOpen)
            {
                _channel = _connection.CreateModel();
                _channel.QueueDeclare("orderQueue", false, false, false, null);
            }

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    // Stop consuming when cancellation is requested
                    return;
                }

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received Order: " + message);

                using var scope = _scopeFactory.CreateScope();
                var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();
                deliveryService.AssignDeliveryPartner(message);
            };

            _channel.BasicConsume(queue: "orderQueue", autoAck: true, consumer: consumer);

            // Wait until cancellation is requested
            await Task.Delay(-1, cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}

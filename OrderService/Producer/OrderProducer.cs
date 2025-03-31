using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace OrderService.Producer
{
    public class OrderProducer: IOrderProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly IConfiguration _configuration;
        public OrderProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:HostName"] };
        }
        public void SendMessage<T>(T message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            string queueName = _configuration["RabbitMQ:QueueName"];
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            Console.WriteLine($" [x] Sent: {json}");
        }
    }
}

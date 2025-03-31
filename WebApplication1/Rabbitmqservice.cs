using RabbitMQ.Client;

namespace WebApplication1
{
    public class Rabbitmqservice
    {
        public void SendMessage<T>(T message)
        {
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = factory.CreateConnectionAsync();
                using var channel = connection.CreateModel();
            }
        }
    }
}

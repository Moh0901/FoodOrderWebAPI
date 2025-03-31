namespace OrderService.Producer
{
    public interface IOrderProducer
    {
        void SendMessage<T>(T message);
    }
}

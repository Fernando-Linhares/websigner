using System.Text;
using Api.Features.Dotenv;
using RabbitMQ.Client;

namespace Api.Features.Queue;

public class Dispatcher
{
    public QueueOutput ExecuteOnQueue(string queueName, IQueuelable queuelable)
    {
        var connection = GetConnection();
        var channel = connection.CreateModel();
        string content = queuelable.Serialize();
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        channel.BasicPublish(
            exchange: "logs",
            routingKey: string.Empty,
            basicProperties: null,
            body: bytes            
            );
        
        return new QueueOutput(queueName, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    private IConnection GetConnection()
    {
        var config = new ConfigApp();
        
        var connectionFactory = new ConnectionFactory
        {
            HostName = config.Get("rabbitmq.host"),
            UserName = config.Get("rabbitmq.user"),
            Password = config.Get("rabbitmq.password"),
        };
        
        return connectionFactory.CreateConnection();
    }
}
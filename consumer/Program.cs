using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Mail;
using Consumer;
using Newtonsoft.Json;

SmtpClient GetClient()
{
    string host, username, password, port;

    host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "localhost";
    port = Environment.GetEnvironmentVariable("SMTP_PORT") ?? "1025";
    username = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? "root";
    password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "d3faultP4ss";

    return new SmtpClient(host, Int32.Parse(port))
    {
        Credentials = new NetworkCredential(username, password),
        EnableSsl = false
    };
}

void SendEmail(MessageDto messageDto, SmtpClient smtp)
{
    var message =  new MailMessage
    {
        Subject = messageDto.Subject,
        From = new MailAddress(messageDto.From),
        Body =  messageDto.Body,
        IsBodyHtml = true,
    };

    message.To.Add(messageDto.To);

    smtp.SendMailAsync(message);
}

IConnection RabbitMQConnect(string user, string password, string host)
{
    ConnectionFactory factory = new ConnectionFactory
    {
        UserName = user,
        Password = password,
        HostName = host
    };
    return factory.CreateConnection();
}
var connection = RabbitMQConnect(
    user: Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "mqrootmanager",
    password: Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "d3faultP4ss",
    host: Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost"
);

string queueName = "signer.email";

using var channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
channel.QueueDeclare(queueName=queueName);

channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

var client = GetClient();
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    byte[] body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var dto = JsonConvert.DeserializeObject<MessageDto>(message);
    SendEmail(dto, client);
    Console.WriteLine(" [x] Received ................ {0}  OK", dto.Subject);
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
Console.WriteLine("[*] Server Running ........... [*]");
while(true)
{
    Console.WriteLine("......");
    Console.ReadLine();
}

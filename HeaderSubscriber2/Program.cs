using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");
using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0, 1, false);
var subscriber = new EventingBasicConsumer(channel);

var queuename = channel.QueueDeclare().QueueName;

/// burada da dinleyeceğimiz header mesajına göre dicrionary oluşturuyoruz 
Dictionary<string, object> headers = new Dictionary<string, object>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "any");

channel.QueueBind(queue: queuename, exchange: "HeaderExchange", routingKey: string.Empty, arguments: headers);

channel.BasicConsume(queue: queuename, autoAck: false, consumer: subscriber);
subscriber.Received += Subscriber_Received; /// 

void Subscriber_Received(object? sender, BasicDeliverEventArgs e)
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
    channel.BasicAck(e.DeliveryTag, false);
}


Console.ReadLine();
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");
using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

//channel.ExchangeDeclare(exchange: "ExchangeAdiGirilecek", type: ExchangeType.Fanout, durable: true); /// İsterseniz Exchange i subscriber da da declare edebilirsiniz tıpkı kuyrukta olduğu gibi 
/// Fakat biz burda şunu biliyoruz Publisher bu Exchange i oluşturdu. bu sebeple kaldırdık 
/// Ama kaldırmasaydıkta hata vermezdi ( kullansanız hata almazsınız ) 

var QueueName = channel.QueueDeclare().QueueName;  /// Kuyruk ismi random olacak. çünkü birden fazla instance oluşturduğumda aynı kuyruk ismi ile bağlanırlar. o yüzden guid yapacaz 

channel.QueueBind(QueueName, "ExchangeAdiGirilecek", string.Empty,null);

channel.BasicQos(0, 1, false);

var subscriber = new EventingBasicConsumer(channel);


channel.BasicConsume(QueueName, false, subscriber);

subscriber.Received += Subscriber_Received; /// 

void Subscriber_Received(object? sender, BasicDeliverEventArgs e)
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));

    channel.BasicAck(e.DeliveryTag,false);
}


Console.ReadLine();
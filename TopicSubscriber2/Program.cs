using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");
using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

/// Burada artık kuyrukları kendimiz oluşutrmamız gerekecektir. 
var quequeName = channel.QueueDeclare().QueueName; /// Burada random bi isim tercih ettim sebebi önemli değil. 
                                                   /// yani isimde verebilirdik ama önemli olan root bizim için


/// Kuyruğumuzu bu aşamada bind edelim. artık bu aşamada oluşan kuyruğu belirli bir route a göre bind edeceğiz. 
/// bizler Category için gelen mesajları dinleyeceğiz. o sebeple arada product olan route u yazıyoruz. 
/// Şimdi nasıl bir route u dinlemek istiyorum ben ? ortası Category olan ı dinle. 
channel.QueueBind(queue: quequeName, exchange: "TopicExchange", routingKey: "*.Category.*", arguments: null);

channel.BasicQos(0, 1, false);
var subscriber = new EventingBasicConsumer(channel);

channel.BasicConsume(queue: quequeName, autoAck: false, consumer: subscriber);
subscriber.Received += Subscriber_Received; /// 

void Subscriber_Received(object? sender, BasicDeliverEventArgs e)
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));

    channel.BasicAck(e.DeliveryTag, false);
}


Console.ReadLine();
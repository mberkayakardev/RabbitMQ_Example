using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;



var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare("Kuyrukadimburaya", true, false, false);  // Eğer Subscriber tarafında sizler bu kodu sildiğinzide 
                                                               // ve uygulamanızı ayağa kaldırdığınızda böyle bir kuyruk RabbitMQ
                                                               // da yoksa hata alırsınız o yüzden yoksa diye her ihtimale karşılık
                                                               // Kuyruğu tanımlıyoruz. Publisher oluşturmazsa bu kuyruğu subscriber 
                                                               // Oluştursun. (Kuyruk oluşturma işlemini ister publisher da zorunlu ,
                                                               // isterseniz subscriber da da yapabilirsin) olması güzel yoksa sorun teşkil etmez


var subscriber = new EventingBasicConsumer(channel);

var response = channel.BasicConsume("Kuyrukadimburaya", false, subscriber); /// verdiğimiz kuyruk ismindeki mesajları alacak

subscriber.Received += Subscriber_Received; /// 

void Subscriber_Received(object? sender, BasicDeliverEventArgs e)
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
}

Console.WriteLine("📡 Dinleme başlatıldı. Çıkmak için Ctrl+C ...");

// Sonsuz bekleme – programı açık tutmak için
while (true)
{
    await Task.Delay(100000); // CPU'yu yormadan döngüde bekler
}
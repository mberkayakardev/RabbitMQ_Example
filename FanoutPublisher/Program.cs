using RabbitMQ.Client;
using System.Text;

SendMessage();

async Task SendMessage()
{
    while (true)
    {
        Console.WriteLine("Lütfen bir mesaj giriniz \n\n ");
        string deger = Console.ReadLine();


        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");
        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();


        //channel.QueueDeclare("Kuyrukadimburaya", true, false, false); /// Bizler Fanout ta kuyruk declare etmeyeceğiz declare etme işlemi Subscriber a ait
                                                                        /// Subscribver kuyruğunu declare edecek ve bizim Exchange e bağlanacak 
        channel.ExchangeDeclare(exchange: "ExchangeAdiGirilecek", type: ExchangeType.Fanout, durable: true); /// durable : true demek fiziksel olarak kaydedilsin RabbitMQ restrat yerse exchange kaybolmasın   

        var messageBody = Encoding.UTF8.GetBytes(deger);

        //channel.BasicPublish(string.Empty, "Kuyrukadimburaya", null, messageBody); // eskiden buraya bizler direk kuyruğa a kendimiz yönlendiriyorduk şimdi sadece exchange e salacaz 

        channel.BasicPublish("ExchangeAdiGirilecek", string.Empty, null, messageBody);  




        Console.WriteLine(" Mesaj Gönderildi. \n\n ");
        Console.WriteLine(" ------------------------------------------------------------------------ ");
    }

}

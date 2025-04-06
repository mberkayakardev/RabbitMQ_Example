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

        channel.QueueDeclare("Kuyrukadimburaya", true, false, false);

        var messageBody = Encoding.UTF8.GetBytes(deger);

        for (var i = 0; i > 5; i ++)
        {
            channel.BasicPublish(string.Empty, "Kuyrukadimburaya", null, messageBody);


        }

        //channel.eq

        Console.WriteLine(" Mesaj Gönderildi. \n\n ");
        Console.WriteLine(" ------------------------------------------------------------------------ ");
    }

}

using RabbitMQ.Client;
using System.Text;

namespace DirectPublisher
{
    enum LogLevel
    {
        Info = 1,
        Exception = 2
    }

    enum Entity
    {
        Stock = 1,
        Category = 2,
        Order = 3

    }

    class Program
    {
        static async Task Main(string[] args)
        {
            await SendMessage();
        }

        static async Task SendMessage()
        {
            while (true)
            {
                #region Ekrandan okuma muhabbetleri 
                Console.WriteLine("Lütfen bir mesaj oluşturmak için enter a bas\n\n ");
                Console.ReadLine();
                #endregion

                //// bir üstte yer alan routing key de bizler artık . lar ile birden fazla route oluşturacak şeklide bölme işlemini gerçekleştirdik. 
                /// Info.Stock.Exception
                /// Exception.Category.Exception gibi routing i random oluşturmak için koydum
               
                Random rnd = new Random();
                LogLevel log1 = (LogLevel)rnd.Next(1, 3);
                LogLevel log2 = (LogLevel)rnd.Next(1, 3);
                Entity entity = (Entity)rnd.Next(1, 4);
                var routingKey = log1+"."+entity+"."+log2;

                Console.WriteLine($"Routing Key in :  {routingKey}   Entity ise : {entity}");


                var factory = new ConnectionFactory();
                factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");
                using var connection = factory.CreateConnection();

                var channel = connection.CreateModel();

                /// Hatırlanacağı üzere bizler Topic Exchange için gidip kendimiz kuyruk ve exchange oluşturuyorduk. 
                /// Exchange in ismi ni ver Tipini değiştirdik. 
                channel.ExchangeDeclare(exchange: "TopicExchange", type: ExchangeType.Topic, durable: true);

                //// Kuyruklar bu senaryoda subscriberlar tarafında oluşturulacağından ötürü bizler kuyruklarımıız direct te olduğu gibi değil kaldırdık direkt

                var messageBody = Encoding.UTF8.GetBytes(routingKey);

                channel.BasicPublish(exchange: "TopicExchange", routingKey: routingKey, basicProperties: null, body: messageBody);

                Console.WriteLine(" Mesaj Gönderildi. \n\n ");
                Console.WriteLine(" ------------------------------------------------------------------------ ");
            }
        }
    }
}









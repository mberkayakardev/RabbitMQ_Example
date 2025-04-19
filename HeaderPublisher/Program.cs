using RabbitMQ.Client;
using System.Text;

namespace DirectPublisher
{

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
                var message = Console.ReadLine();
                #endregion

                var factory = new ConnectionFactory();
                factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");
                using var connection = factory.CreateConnection();

                var channel = connection.CreateModel();

                /// Exchange tüpünmizi bu aşamadan sonra header exchange seçelim
                channel.ExchangeDeclare(exchange: "HeaderExchange", type: ExchangeType.Headers, durable: true);

                /// Mesajı göndermeden önce header ımızı belirliyoruz.
                /// key string, value object olarak belirledik. 
                /// object olduğu için herşeyi gömebilirsin, image pdf, nesne .ççç
                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("format", "pdf");
                headers.Add("shape", "a4");


                /// Header bilgilerini yollamak için basic property oluşyuruyoruz. 
                var properties = channel.CreateBasicProperties();
                properties.Headers = headers;


                /// Bu aşamada mesajımızı publish edeceğiz. 
                /// Root vermiyoruz header ile göndereceğiz o yüzden root boş 
                /// header i basic property ile göndereceğiz. 
                channel.BasicPublish(exchange: "HeaderExchange", routingKey: string.Empty, basicProperties: properties, body: Encoding.UTF8.GetBytes(message));



                Console.WriteLine(" Mesaj Gönderildi. \n\n ");
                Console.WriteLine(" ------------------------------------------------------------------------ ");
            }
        }
    }
}









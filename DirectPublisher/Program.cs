using RabbitMQ.Client;
using System.Text;

namespace DirectPublisher
{
    enum LogLevel
    {
        Info = 1,
        Exception = 2
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

                Console.WriteLine("Lütfen bir mesaj giriniz \n\n ");
                string deger = Console.ReadLine();
                Console.WriteLine("Lütfen Mesajın Gönderileceği Route u girin 1 : Exception, 2 : warnin ");
                string deger2 = Console.ReadLine();


                bool parseSuccess = int.TryParse(deger2, out int routeSecimi);
                if (!parseSuccess || !Enum.IsDefined(typeof(LogLevel), routeSecimi))
                {
                    Console.WriteLine("Geçersiz seçim yapıldı. Lütfen 1 ya da 2 giriniz.");
                    continue;
                }

                var selectedLogLevel = (LogLevel)routeSecimi;
                string routingKey = selectedLogLevel.ToString();
                #endregion


                var factory = new ConnectionFactory();
                factory.Uri = new Uri("amqps://lluykbjx:58YumjfZikq5pEOuqhXKxvMugdAm-nS6@collie.lmq.cloudamqp.com/lluykbjx");
                using var connection = factory.CreateConnection();

                var channel = connection.CreateModel();

                /// Hatırlanacağı üzere bizler Direct Exchange için gidip kendimiz kuyruk ve exchange oluşturuyorduk. 
                /// Exchange in ismi ni ver Tipini değiştirdik. 
                channel.ExchangeDeclare(exchange: "DirectExchange", type: ExchangeType.Direct, durable: true);


                /// Kuyruklarımız Direct Exchange te kendimiz oluşturacağız 
                /// Her bir log yada işlem tipi için hem route hem de kuyruk oluşturacağız publisher tarafında 
                /// Exclusevie false : farklı channelerdan da bağlanabileyim.
                /// AutoDelete : false bu kuyruğa subscribe olan bir arkadaş yoksa yinede kalsın.
                if (routingKey == LogLevel.Exception.ToString() )
                {
                    var QueueNameException = channel.QueueDeclare(queue: "Exception", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    /// Bu aşamada artık Hem Kuyruklar Declare edilecek Hem de Kuyruklar Exchange e  bind edilip routing verilecek 
                    channel.QueueBind(queue: QueueNameException, exchange: "DirectExchange", routingKey: routingKey);
                }
                if (routingKey == LogLevel.Info.ToString())
                {
                    var QueueNameInfo = channel.QueueDeclare(queue: "Info", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    /// Bu aşamada artık Hem Kuyruklar Declare edilecek Hem de Kuyruklar Exchange e  bind edilip routing verilecek 
                    channel.QueueBind(queue: QueueNameInfo, exchange: "DirectExchange", routingKey: routingKey);
                }

                var messageBody = Encoding.UTF8.GetBytes(deger);

                /// Bu aşamadan sonra route oluşturmamız gerekecektir. route u klavyeden aldığımız için exchange  + routekey şeklinde ilerleyebiliriz. 
                /// oluşturulan mesajlar exchange üzerinden hangi route a gideceğini belirleyeceğiz.
                /// mesajları publish ederken hangi route a gideceğini de belirlememiz gerekecektir. 
                /// mesajları publish etmeden önce route unu da belirleyeceğiz ki direct exchange gelen route a göre ilgili kuyruğa gönderecek. 
                channel.BasicPublish(exchange: "DirectExchange", routingKey: routingKey, basicProperties: null, body: messageBody);

                Console.WriteLine(" Mesaj Gönderildi. \n\n ");
                Console.WriteLine(" ------------------------------------------------------------------------ ");
            }
        }
    }
}


 
 

 
    


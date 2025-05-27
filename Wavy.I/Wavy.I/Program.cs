using RabbitMQ.Client;
using System.Text;
using Wavy.I;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography.X509Certificates;

string path = "C:/Users/Alp3x/Desktop/Alp3x/01_UTAD/02_2Ano/2Sem//LAb/utad.ToDo/SDProjectII/Wavy.I/Wavy.I/Data.json";

string Id = "Wavy1";

var dadosJson = File.ReadAllText(path);

var dados = JsonConvert.DeserializeObject<List<Data>>(dadosJson);

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

foreach(var item in dados)
{
   string message = Id + "-" + nameof(item.Temperatura) + ":" + item.Temperatura + " "
        + nameof(item.VelocidadeVento) + ":" + item.VelocidadeVento + " "
        + nameof(item.EnergiaProd) + ":" + item.EnergiaProd;

    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
    Console.WriteLine($" [x] Sent \n {message}");
    Thread.Sleep(10000);
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
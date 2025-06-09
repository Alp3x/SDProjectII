using RabbitMQ.Client;
using System.Text;
using Wavy.I;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography.X509Certificates;

string path = "./Wavy.I/Wavy.I/Data.json";

string Id = "Wavy1";

var dadosJson = File.ReadAllText(path);

var dados = JsonConvert.DeserializeObject<List<Data>>(dadosJson);

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

foreach (var item in dados)
{
    foreach (var ite in dados)
    {
        string message = Id + "-" + nameof(ite.Temperatura) + ":" + ite.Temperatura + " "
             + nameof(ite.VelocidadeVento) + ":" + ite.VelocidadeVento + " "
             + nameof(ite.EnergiaProd) + ":" + ite.EnergiaProd;

        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
        Console.WriteLine($" [x] Sent \n {message}");
        Thread.Sleep(1000);
    }
}
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
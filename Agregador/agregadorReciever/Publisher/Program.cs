using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        string path = @"C:\Users\Filipe\Documents\Sistemas Distribuídos\RepoTrabalho2\SDProjectII\Agregador\agregadorReciever\agregadorReciever\UpdatedData.json";
        string Id = "Agregador1";

        var dadosJson = File.ReadAllText(path);
        Console.WriteLine(dadosJson);
        var dados = JsonConvert.DeserializeObject<List<Data>>(dadosJson) ?? new List<Data>();

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

        foreach (var item in dados)
        {
            string message = Id + "-" +
                nameof(item.Temperatura) + ":" + item.Temperatura + " " +
                nameof(item.VelocidadeVento) + ":" + item.VelocidadeVento + " " +
                nameof(item.EnergiaProd) + ":" + item.EnergiaProd;

            var body = Encoding.UTF8.GetBytes(message);

           await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
            Console.WriteLine($" [x] Sent: {message}");

            await Task.Delay(1000); // versão assíncrona de Thread.Sleep
        }
    }
}

public class Data
{
    public float Temperatura { get; set; }
    public float VelocidadeVento { get; set; }
    public float EnergiaProd { get; set; }
}
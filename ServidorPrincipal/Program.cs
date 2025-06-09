
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Program
{
    static List<Data> dadosRecebidos = new List<Data>();
    static string path = @"C:\Users\Filipe\Documents\Sistemas Distribuídos\RepoTrabalho2\SDProjectII\ServidorPrincipal\ReceivedData.json";

    public static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");

            try
            {
                 var id = message.Split("-")[0];
                Console.WriteLine("Id->" + id);
                var data = message.Split("-")[1];
                var temp = message.Split(":")[1].Split(" ")[0];
                Console.WriteLine("temp->" + temp);
                var dataPosTemp = message.Split(":")[1].Split(" ")[1];
                var velVento = message.Split(":")[2].Split(" ")[0];
                Console.WriteLine("velVento->" + velVento);
                var posVento = message.Split(":")[2].Split(" ")[1];
                var energiaProd = message.Split(":")[3].Split(" ")[0];
                Console.WriteLine("energiaProd->" + energiaProd);

                var item = new Data
                {
                    Id = id,
                    Temperatura = float.Parse(temp),
                    VelocidadeVento = float.Parse(velVento),
                    EnergiaProd = float.Parse(energiaProd)
                
                };

                List<Data> dados;
                if (File.Exists(path))
                {
                    var dadosJson =  File.ReadAllText(path);
                    dados = JsonConvert.DeserializeObject<List<Data>>(dadosJson) ?? new List<Data>();
                }
                else
                {
                    dados = new List<Data>();
                }

                dados.Add(item);

                var updatedJson = JsonConvert.SerializeObject(dados, Formatting.Indented);
                 File.WriteAllText(path, updatedJson);
                Console.WriteLine("Data saved to ReceivedData.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao processar mensagem: " + ex.Message);
            }

            // Done
            return Task.CompletedTask;
    
        };

        await channel.BasicConsumeAsync(queue: "hello", autoAck: true, consumer: consumer);

        Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
        Console.ReadLine(); 
    }

}

public class Data
{
    public string Id { get; set; }
    public float Temperatura { get; set; }
    public float VelocidadeVento { get; set; }
    public float EnergiaProd { get; set; }
}
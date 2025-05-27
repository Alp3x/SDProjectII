using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using agregadorReciever;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.IO;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
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
        Temperatura = float.Parse(temp),
        VelocidadeVento = float.Parse(velVento),
        EnergiaProd = float.Parse(energiaProd)
    };

    string docPathRead = "C:/Users/Alp3x/Desktop/Alp3x/01_UTAD/02_2Ano/2Sem/LAb/utad.ToDo/SDProjectII/agregadorReciever/agregadorReciever/RawData.json";
    string docPathWrite = "C:/Users/Alp3x/Desktop/Alp3x/01_UTAD/02_2Ano/2Sem/LAb/utad.ToDo/SDProjectII/agregadorReciever/agregadorReciever/";

    var dadosJson = File.ReadAllText(docPathRead);

    var dados = JsonConvert.DeserializeObject<List<Data>>(dadosJson);

    dados.Add(item);
    File.WriteAllText(docPathRead, "");

    using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPathWrite, "RawData.json")))
    {
        var json = JsonConvert.SerializeObject(dados, Formatting.Indented);
        outputFile.WriteLine(json);
        Console.WriteLine("Data saved to RawData.json");
    }

    return Task.CompletedTask;
};


await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);




Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
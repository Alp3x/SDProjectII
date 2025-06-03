using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;
using AgregadorGrpc;
using Grpc.Net.Client;
using Grpc.Core;
using Newtonsoft.Json;
using System.Data.Common;

var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

string docPathRead = "C:/Users/Alp3x/Desktop/Alp3x/01_UTAD/02_2Ano/2Sem/LAb/utad.ToDo/SDProjectII/Agregador/agregadorReciever/agregadorReciever/RawData.json";
var dadosJson = File.ReadAllText(docPathRead);
var dados = JsonConvert.DeserializeObject<List<DataModel>>(dadosJson);

var channel = GrpcChannel.ForAddress("https://localhost:7128", new GrpcChannelOptions { HttpHandler = handler });
var client = new Greeter.GreeterClient(channel);
if(dados == null || dados.Count == 0)
{
    Console.WriteLine("No data found in the JSON file.");
    return;
}

    List<DataModel> updatedData = new List<DataModel>();

foreach (var item in dados)
{
        var reply = await client.SayHelloAsync(new HelloRequest { Name = "Agregador", Temp = item.Temperatura, EnergiaProd = item.EnergiaProd, VelocidadeVento = item.VelocidadeVento });
        Console.WriteLine("Miles -> " + reply.Message);
        var updatedItem = new DataModel
        {
            Temperatura = item.Temperatura,
            EnergiaProd = item.EnergiaProd,
            VelocidadeVento = item.VelocidadeVento
        };
        updatedData.Add(updatedItem);
}

string docPathWrite = "C:/Users/Alp3x/Desktop/Alp3x/01_UTAD/02_2Ano/2Sem/LAb/utad.ToDo/SDProjectII/Agregador/agregadorReciever/agregadorReciever/UpdatedData.json";

string jsonOutput = JsonConvert.SerializeObject(updatedData, Formatting.Indented);
File.WriteAllText(docPathWrite, jsonOutput);
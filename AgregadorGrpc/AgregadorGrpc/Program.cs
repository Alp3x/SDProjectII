using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;
using AgregadorGrpc;
using Grpc.Net.Client;
using Grpc.Core;

var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://localhost:7128", new GrpcChannelOptions { HttpHandler = handler });
var client = new Greeter.GreeterClient(channel);

Console.WriteLine("KM to Mile Converter");
Console.Write("Km -> ");
var input = Console.ReadLine();

if (double.TryParse(input, out double km))
{
    var reply = await client.SayHelloAsync(new HelloRequest { Name = "Agregador", Km = (float) km });
    Console.WriteLine("Miles -> " + reply.Message);
}
else
{
    Console.WriteLine("Invalid input. Please enter a valid number.");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
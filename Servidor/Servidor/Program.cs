using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Core;
using Servidor;

var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://localhost:7187", new GrpcChannelOptions { HttpHandler = handler });
var client = new Greeter.GreeterClient(channel);

Console.WriteLine("KM to Mile Converter");
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Servidor"});
Console.WriteLine("-> " + reply.Message);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
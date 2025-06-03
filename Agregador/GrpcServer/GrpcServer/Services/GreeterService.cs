using Grpc.Core;
using GrpcServer;

namespace GrpcServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = request.Name + "This is your data Changed" + (request.Temp + 237) + request.EnergiaProd * 1.5 + request.VelocidadeVento * 2
            });
        }
    }
}

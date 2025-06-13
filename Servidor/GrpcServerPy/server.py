import grpc
from concurrent import futures
import service_pb2
import service_pb2_grpc
import statistics

class MyService(service_pb2_grpc.MyServiceServicer):
    def SayHello(self, request, context):
        return service_pb2.HelloReply(message=f"Hello, {request.name} from Python!")

    def ComputeStats(self, request, context):
        temps = [d.temperatura for d in request.dados]
        ventos = [d.velocidade_vento for d in request.dados]
        energias = [d.energia_prod for d in request.dados]

        # Cálculo das médias
        media_temp = statistics.mean(temps) if temps else 0.0
        media_vento = statistics.mean(ventos) if ventos else 0.0
        media_energia = statistics.mean(energias) if energias else 0.0

        # Simples previsão: último valor + 1 (só exemplo)
        forecast = f"Prevista temperatura: {temps[-1] + 0.5}" if temps else "Sem dados"

        return service_pb2.StatsReply(
            media_temperatura=media_temp,
            media_vento=media_vento,
            media_energia=media_energia,
            forecast=forecast
        )

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    service_pb2_grpc.add_MyServiceServicer_to_server(MyService(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()

if __name__ == '__main__':
    serve()
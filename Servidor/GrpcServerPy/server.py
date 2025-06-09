import grpc
from concurrent import futures
import service_pb2
import service_pb2_grpc

class MyService(service_pb2_grpc.MyServiceServicer):
    def SayHello(self, request, context):
        return service_pb2.HelloReply(message=f"Hello, {request.name} from Python!")

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    service_pb2_grpc.add_MyServiceServicer_to_server(MyService(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()

if __name__ == '__main__':
    serve()

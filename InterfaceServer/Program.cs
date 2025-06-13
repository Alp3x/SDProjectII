using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Grpc.Net.Client;
using Example; 
using Newtonsoft.Json;

public class DataJson
{
    public string Id { get; set; }
    public float Temperatura { get; set; }
    public float VelocidadeVento { get; set; }
    public float EnergiaProd { get; set; }
}

public class CLI
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new MyService.MyServiceClient(channel);

        while (true)
        {
            Console.Write("Comando (mean/forecast/anomalies/exit): ");
            var cmd = Console.ReadLine()?.Trim();

            if (cmd == "exit")
                break;

            if (cmd == "mean" || cmd == "forecast")
            {
                try
                {
                    var dataJson = File.ReadAllText(@"C:\Users\Filipe\Documents\Sistemas Distribuídos\RepoTrabalho2\SDProjectII\ServidorPrincipal\ReceivedData.json");
                    var dados = JsonConvert.DeserializeObject<List<DataJson>>(dataJson);

                    var dataList = new DataList();
                    foreach (var d in dados)
                    {
                        dataList.Dados.Add(new Data
                        {
                            Id = d.Id,
                            Temperatura = d.Temperatura,
                            VelocidadeVento = d.VelocidadeVento,
                            EnergiaProd = d.EnergiaProd
                        });
                    }

                    var reply = await client.ComputeStatsAsync(dataList);

                    if (cmd == "mean")
                    {
                        Console.WriteLine($"Média Temperatura: {reply.MediaTemperatura}");
                        Console.WriteLine($"Média Velocidade Vento: {reply.MediaVento}");
                        Console.WriteLine($"Média Energia Produzida: {reply.MediaEnergia}");
                    }
                    else if (cmd == "forecast")
                    {
                        Console.WriteLine($"Previsão: {reply.Forecast}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else if (cmd == "anomalies")
            {
                try
                {
                    var dataJson = File.ReadAllText(@"C:\Users\Filipe\Documents\Sistemas Distribuídos\RepoTrabalho2\SDProjectII\ServidorPrincipal\ReceivedData.json");
                    var dados = JsonConvert.DeserializeObject<List<DataJson>>(dataJson);

                    var dataList = new DataList();
                    foreach (var d in dados)
                    {
                        dataList.Dados.Add(new Data
                        {
                            Id = d.Id,
                            Temperatura = d.Temperatura,
                            VelocidadeVento = d.VelocidadeVento,
                            EnergiaProd = d.EnergiaProd
                        });
                    }

                    var reply = await client.DetectAnomaliesAsync(dataList);

                    if (reply.Anomalies.Count == 0)
                    {
                        Console.WriteLine("✅ Nenhuma anomalia detetada.");
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Anomalias encontradas:");
                        foreach (var anomaly in reply.Anomalies)
                        {
                            Console.WriteLine(anomaly);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }

            else
            {
                Console.WriteLine("Comando inválido.");
            }
        }
    }
}

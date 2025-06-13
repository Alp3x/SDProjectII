using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

public class CLI
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Write("Comando (mean/forecast/exit): ");
            var cmd = Console.ReadLine()?.Trim();

            if (cmd == "exit") break;

            if (cmd == "mean" || cmd == "forecast")
            {
                var dataJson = File.ReadAllText("./Agregador/agregadorReciever/agregadorReciever/ReceivedData.json");

                using var http = new HttpClient();
                var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                var response = await http.PostAsync($"http://localhost:5000/api/{cmd}", content);
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Resultado: {result}");
            }
            else
            {
                Console.WriteLine("Comando inválido.");
            }
        }
    }
}
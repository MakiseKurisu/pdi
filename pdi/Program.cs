using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using pdi.Web;

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace pdi
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            var daemon = new Command("daemon", "Start web daemon");
            daemon.Handler = CommandHandler.Create(async (CancellationToken token) =>
            {
                await Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                    .Build()
                    .RunAsync(token);
            });

            var show = new Command("show", "Show cluster information");

            var root = new RootCommand("Proton Distributed Infrastructure");
            root.Add(daemon);
            root.Add(show);

            // Parse the incoming args and invoke the handler
            return await root.InvokeAsync(args);
        }
    }
}

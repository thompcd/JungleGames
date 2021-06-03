using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace SnekWrastler
{
    public static class Program
    {
        private static readonly CancellationTokenSource TokenSource;

        static Program()
        {
            TokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, args) =>
            {
                TokenSource.Cancel();
                args.Cancel = true;
            };
        }

        public static void Main(string[] args) =>
            MainAsync(args, TokenSource.Token).ConfigureAwait(true);

        private static async Task MainAsync(string[] args, CancellationToken ct)
        {
            var builder = new HostBuilder()
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddJsonFile("appsettings.json", optional: true);
                  config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                  config.AddEnvironmentVariables(prefix: "PREFIX_");
                  if (args != null)
                  {
                      config.AddCommandLine(args);
                  }
              })
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddOptions();
                  services.Configure<Snek>(hostContext.Configuration.GetSection("SnekConfig"));
                  services.AddSingleton<IHostedService, Snek>();
              })
              .ConfigureLogging((hostingContext, logging) => {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
              });

            await builder.RunConsoleAsync();
        }
    }
}

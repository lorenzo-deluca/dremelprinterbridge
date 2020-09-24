using DremelPrinterBridge.Core.Entities;
using DremelPrinterBridge.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace DremelPrinterBridge.Core.HostedService
{
    public class RefreshMemoryHostedService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<RefreshMemoryHostedService> logger;

        public RefreshMemoryHostedService(
            IServiceProvider serviceProvider,
            ILogger<RefreshMemoryHostedService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        #region Hosted Service Common
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[{nameof(RefreshMemoryHostedService)}] Starting service.");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[{nameof(RefreshMemoryHostedService)}] Executing service.");
            await StartServiceAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[{nameof(RefreshMemoryHostedService)}] Stopping service.");
            await base.StopAsync(cancellationToken);
        }
        #endregion

        private async Task<DremelPrinterStatusModel> GetPrinterStatus()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://192.168.1.110/command"))
                {
                    request.Content = new StringContent($"GETPRINTERSTATUS={Uri.EscapeDataString("")}");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);

                    return JsonConvert.DeserializeObject<DremelPrinterStatusModel>(await response.Content.ReadAsStringAsync());
                }
            }
        }

        private async Task StartServiceAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[{nameof(RefreshMemoryHostedService)}] Service runs every minute.");

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(10000, cancellationToken);

                try
                {
                    var printerStatus = await GetPrinterStatus();
                    if (printerStatus == null)
                        continue;

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetService<InMemoryContextDb>();

                        if (context.Printers.Any())
                        {
                            var printer = context.Printers.FirstOrDefault();

                            printer.Update(printerStatus.firmware_version, printerStatus.door_open,
                               printerStatus.elaspedtime, printerStatus.progress, printerStatus.remaining,
                               printerStatus.totalTime, printerStatus.jobname, printerStatus.jobstatus,
                               printerStatus.temperature, printerStatus.extruder_target_temperature, 
                               printerStatus.platform_temperature, printerStatus.buildPlate_target_temperature,
                               printerStatus.chamber_temperature, printerStatus.filament_type, 
                               printerStatus.layer, printerStatus.status);
                        }
                        else
                        {
                            context.Printers.Add(new Printer());
                        }

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"[{nameof(RefreshMemoryHostedService)}] Error on execute CheckExpiredRequest. {ex.Message}");
                }
            }
        }
    }
}
using DremelPrinterBridge.Database;
using DremelPrinterBridge.Core.Configuration;
using DremelPrinterBridge.Core.HostedService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace DremelPrinterBridge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();

            services.AddDbContext<InMemoryContextDb>(opt => opt.UseInMemoryDatabase("DremelPrinterBridgeDb"));

            services.Configure<DremelPrinterConfigurator>(options
               => Configuration.GetSection(nameof(DremelPrinterConfigurator)).Bind(options));

            services.AddHostedService<RefreshMemoryHostedService>();

            Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Information()
                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Error)
                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                     .WriteTo.File($"{nameof(DremelPrinterBridge)}-log.txt")
                     .Enrich.FromLogContext()
                     .Enrich.WithExceptionDetails()
                       .CreateLogger();
            Log.Information($"{nameof(DremelPrinterBridge)} Startup");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

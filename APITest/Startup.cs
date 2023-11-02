using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
//using APITest.Controllers;
using APITest.Migrations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace APITest
{
    public class Startup
    {
        public Startup(IHostEnvironment environment)
        {
            // Configure Serilog here
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Debug()
                .CreateLogger();

            if (environment.IsDevelopment())
            {
                Log.Information("Running in Development mode.");
            }
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    Log.Information("Configure is running with swagger");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                builder
                   .WithOrigins("http://localhost:4200", "https://localhost:4200")
                   .SetIsOriginAllowedToAllowWildcardSubdomains()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                   .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Log.Information("Configure is running");
        }
    }
}

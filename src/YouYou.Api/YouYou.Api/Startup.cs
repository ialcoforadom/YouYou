using YouYou.Api.Configuration;
using YouYou.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YouYou.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<YouYouContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Default"));
                // .EnableSensitiveDataLogging()//Para habilitar log do entity nas consultas
                //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            });

            //services.AddRedisConfig(Configuration);

            services.AddIdentityConfiguration(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddGlobalization();

            services.WebApiConfig();

            services.AddSwaggerConfig();

            services.AddHttpContextAccessor();

            services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Staging");
                app.UseDeveloperExceptionPage();
                // app.UseHsts();
            }

            //app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseGlobalization();

            app.UseAuthentication();

            app.UseIdentityConfiguration();

            // app.UseMiddleware<ExceptionMiddleware>();

            app.UseProblemDetailsExceptionHandler(loggerFactory);

            app.UseApiConfig();

            app.UseSwaggerConfig();
        }
    }
}

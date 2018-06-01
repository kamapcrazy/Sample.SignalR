using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Sample.SignalR.Services.Implementations;
using Sample.SignalR.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace Sample.SignalR
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
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddMvc();

            services.AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "SignalR", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();


            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/signalr/notification");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "SignalR"); });
        }
    }
}

using Basket.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching;

using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using EventBusRabbitMQ;
//using RabbitMQ.Client;
//using EventBusRabbitMQ.Producer;

namespace Basket.API
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

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            //services.AddSingleton<ConnectionMultiplexer>(sp =>
            //{
            //    var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
            //    return ConnectionMultiplexer.Connect(configuration);
            //});


            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup));

            //services.AddSingleton<IRabbitMQConnection>(Span =>
            //{
            //    var factory = new ConnectionFactory()
            //    {
            //        HostName = Configuration["EventBus:Hostname"]
            //    };
            //    if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
            //    {
            //        factory.UserName = Configuration["EventBus:UserName"];
            //    }
            //    if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
            //    {
            //        factory.Password = Configuration["EventBus:Password"];
            //    }

            //    return new RabbitMQConnection(factory);
            //});

            //services.AddSingleton<EventBusRabbitMQProducer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
            app.UseSwagger();
            app.UseSwaggerUI(
                c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1"); });

            
        }
    }
}

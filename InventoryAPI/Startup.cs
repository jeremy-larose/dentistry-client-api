using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using InventoryAPI.Repositories;
using InventoryAPI.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

/*
 * Docker Startup Command:
 * docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=net5tutorial mongo
 *
 * From Root Directory:
 * docker build -f InventoryAPI\Dockerfile --force-rm -t catalog:v1 .
 * 
 * Production:
 * docker run -it --rm -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network net5tutorial catalog:v1
 *
 * From Docker Hub:
 * docker run -it --rm -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network net5tutorial jeremylarose/catalog:v1
 */

namespace InventoryAPI
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
            BsonSerializer.RegisterSerializer( new GuidSerializer( BsonType.String ));
            BsonSerializer.RegisterSerializer( new DateTimeOffsetSerializer( BsonType.String ));

            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            });

            services.AddSingleton<IClientRepository, MongoDBClientsRepository>();
            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "InventoryAPI", Version = "v1"});
            });
            services.AddHealthChecks().AddMongoDb(
                mongoDbSettings.ConnectionString,
                name: "mongodb", 
                timeout: TimeSpan.FromSeconds(3),
                tags: new[] { "ready" } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryAPI v1"));
            }
            
            if( env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains( "ready" ),
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonSerializer.Serialize(new
                        {
                            status = report.Status.ToString(),
                            checks = report.Entries.Select(entry => new
                            {
                                name = entry.Key,
                                status = entry.Value.Status.ToString(),
                                exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                duration = entry.Value.Duration.ToString()
                            })
                        });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (_) => false 
                });
            });
        }
    }
}
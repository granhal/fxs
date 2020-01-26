using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using interviewFXS.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace interviewFXS
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
            //MongoClient mongoClient = new MongoClient(ConnectionUtils.ConnectionString(Configuration));
            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionUtils.ConnectionString(Configuration)));
            clientSettings.SslSettings = new SslSettings();
            clientSettings.SslSettings.CheckCertificateRevocation = false;
            clientSettings.UseTls = true;
            clientSettings.VerifySslCertificate = false;
            clientSettings.SslSettings.ClientCertificates = new List<X509Certificate>()
            {
                new X509Certificate(@"cert.pem")
            };

            var mongoClient = new MongoClient(clientSettings);
            services.AddSingleton(mongoClient);

            CronUtils cronUtils = new CronUtils(Configuration, mongoClient);
            cronUtils.Start();

            services.AddControllers();
            services.AddMvc();
            services.AddMvcCore()
                .AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InterviewTemplate.Api", Version = "v1" });
                try{
                    c.IncludeXmlComments(Path.GetFullPath((File.Exists(@"interviewFXS.xml")) ? @"interviewFXS.xml" : @"interviewFXS.xml"));
                }
                catch
                {
                    Console.Write("Not file .xml swagger api found");
                }
            });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}

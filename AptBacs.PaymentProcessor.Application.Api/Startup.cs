using System;
using System.IO;
using System.Reflection;
using AptBacs.PaymentProcessor.Application.Services;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces;
using AptBacs.PaymentProcessor.Domain.RepositoryInterfaces;
using AptBacs.PaymentProcessor.Infrastructure.Repositories.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace AptBacs.PaymentProcessor.Application.Api
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
            RegisterDependencyInjectionInstances(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            RegisterSwaggerApiDocsGenerator(services);   
        }

        private void RegisterDependencyInjectionInstances(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IBacsPaymentQueryService), typeof(BacsPaymentQueryService), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IProcessBacsPaymentService), typeof(ProcessBacsPaymentService), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IPaymentRequestRepository), typeof(PaymentRequestRepository), ServiceLifetime.Singleton));
        }

        private void RegisterSwaggerApiDocsGenerator(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "AptBacs Payment Api", Version = "v1",
                    Description = "AptBacs Payment Processing Api",
                    Contact = new Contact
                    {
                        Name = "Dean Havelock",
                        Email = "deanhavelock@hotmail.com",
                        Url = "https://github.com/DeanHavelock"
                    },
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            ConfigureSwagger(app);
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AptBacs Payment Api - V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;

namespace Violet.WorkItems.Service
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
            var workItemsConfig = Configuration.GetSection("WorkItems");
            var browserEndpointConfig = workItemsConfig.GetSection("BrowserEndpoint");

            services.AddControllers()
                    .AddJsonOptions(opts =>
                    {
                        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });

            services.AddSwaggerGen();

            services.AddSingleton<IDataProvider, InMemoryDataProvider>()
                    .AddSingleton<IDescriptorProvider>(serviceProvider => new Violet.WorkItems.Types.CommonSdlc.CommonSdlcDescriptorProvider())
                    .AddSingleton<WorkItemManager>();

            var origins = browserEndpointConfig.GetSection("Origin").GetChildren().AsEnumerable().Select(c => c.Value).ToArray();
            services.AddCors(options => options
                        .AddDefaultPolicy(builder => builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins(origins)
                            .AllowCredentials()));


            if (workItemsConfig.GetSection("AccessToken").GetValue<bool>("Enabled"))
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var providerConfig = workItemsConfig
                                                .GetSection("AccessToken")
                                                .GetSection("Providers:0");

                        options.Authority = providerConfig.GetValue<string>("Authority");

                        options.TokenValidationParameters.ValidateAudience = false;

                        if (providerConfig.GetValue<bool>("HasInvalidCertificate") is true)
                        {
                            options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                        }
                    });

                services.AddAuthorization(options => options.AddPolicy("WorkItemPolicy", policy => policy.RequireClaim("scope", "workitems")));
            }
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

            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

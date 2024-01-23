using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Violet.WorkItems.Service;
using Violet.WorkItems;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Query;
using Violet.WorkItems.Types;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        QuerySerialization.ConfigureJsonOptions(opts.JsonSerializerOptions);
    });
builder.Services.AddSingleton<IDataProvider>(new FileSystemDataProvider("./sample"))
                .AddSingleton<IDescriptorProvider>(serviceProvider => new Violet.WorkItems.Types.CommonSdlc.CommonSdlcDescriptorProvider())
                .AddSingleton<WorkItemManager>()
                .AddHostedService<WorkItemHost>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var workItemsConfig = configuration.GetSection("WorkItems");
var browserEndpointConfig = workItemsConfig.GetSection("BrowserEndpoint");

var origins = browserEndpointConfig.GetSection("Origin").GetChildren().AsEnumerable().Select(c => c.Value).ToArray();
builder.Services.AddCors(options => options
                    .AddDefaultPolicy(builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(origins)
                        .AllowCredentials()));

var uris = workItemsConfig.GetSection("ApiEndpoint").GetValue<string>("Uri");
builder.WebHost.UseUrls(uris);

if (workItemsConfig.GetSection("AccessToken").GetValue<bool>("Enabled"))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

    builder.Services.AddAuthorization(options => options.AddPolicy("WorkItemPolicy", policy => policy.RequireClaim("scope", "workitems")));
}
else
{
    builder.Services.AddAuthorization(options => options.AddPolicy("WorkItemPolicy", policy => policy.RequireAssertion(x => true)));
}


var app = builder.Build();

app.UseRouting();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

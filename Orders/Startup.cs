using System;
using System.Threading.Tasks;
using Grpc.Core;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Orders.Middlewares;
using ProductService;
using Serilog;

namespace Orders
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

            services.AddLogging(conf =>
            {
                conf.AddConsole();
            });
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromSeconds(5),
                        ValidateAudience = false
                    };
                    
                    config.Authority = "https://localhost:5000";

                    config.SaveToken = true;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders", Version = "v1" });
            });
            
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
            
            services.AddAccessTokenManagement(options =>
                {
                    options.Client.Clients.Add("tokenClient", new ClientCredentialsTokenRequest
                    {
                        Address = "https://localhost:5000/connect/token",
                        ClientId = "orders.client",
                        ClientSecret = "orders.secret",
                        Scope = "api" // optional
                    });
                })
                .ConfigureBackchannelHttpClient();
            
            // token management - delegating handler 
            services.AddClientAccessTokenClient("client", configureClient: client =>
            {
                client.BaseAddress = new Uri("https://localhost:5003");
            });

            services.AddGrpcClient<Greeter.GreeterClient>(o =>
                {
                    o.Address = new Uri("https://localhost:5003");
                    
                }).AddHttpMessageHandler<ClientAccessTokenHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders v1");
                    c.DisplayRequestDuration();
                });
            }

            app.UseHttpsRedirection();
            
            app.UseSerilogRequestLogging();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
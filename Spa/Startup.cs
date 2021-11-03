using System;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Spa
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
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddAuthorization();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                    options.DefaultSignOutScheme = "oidc";
                })
                .AddCookie("Cookies", options =>
                {
                    // host prefixed cookie name
                    options.Cookie.Name = "__BFF-spa";

                    // strict SameSite handling
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    
                    // sliding or absolute
                    options.SlidingExpiration = true;
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "http://localhost:5000";

                    options.ClientId = "spa";
                    options.ClientSecret = "spa.secret";
                    options.ResponseType = "code";

                    options.ResponseMode = "query";

                    options.RequireHttpsMetadata = false;

                    options.MapInboundClaims = false;
                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                });

            services.AddBff(options =>
                {
                    // default value
                    options.ManagementBasePath = "/bff";
                    options.AntiForgeryHeaderName = "x-csrf";
                    options.AntiForgeryHeaderValue = "1";
                })
                .AddRemoteApis();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseBff();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBffManagementEndpoints();

                endpoints.MapRemoteBffApiEndpoint(
                        "/orders-module", "http://localhost:5002", requireAntiForgeryCheck: false)
                    .RequireAccessToken();
            });
            
            app.UseWebSockets();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
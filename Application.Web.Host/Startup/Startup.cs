using Api.Helper.ContentWrapper.Core.Extensions;
using Api.Helper.ContentWrapper.Core.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KNN.NULLPrinter
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(
                  options =>
                  {
                      options.Filters.Add(typeof(ModelStateFeatureFilter));
                      //options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
                      //options.OutputFormatters.Add(new PascalCaseJsonProfileFormatter());
                  })
             .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
             .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //add your settings
            OptionConfigurer.Configure(services, Configuration);

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                                // CORS:Url in appsettings.json can contain more than one address separated by comma.
                                Configuration.GetSection("CORS:Url").Value
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => Regex.Replace(o, @"/", ""))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );
            DependancyRegistrar.Register(services, Configuration);
            AuthConfigurer.Configure(services, Configuration);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Web Api",
                    Description = "All Apis Docs",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Pradeep Raj Thapaliya",
                        Email = "pradeep.thapaliya@amniltech.com",
                        Url = "https://www.pradeeprajthapaliya.com.np"
                    },
                    License = new License
                    {
                        Name = "Use under MIT License",
                        Url = "#"
                    }
                });

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);

                //c.AddSecurityDefinition("jwt", new ApiKeyScheme
                //{
                //    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                //    In = "header",
                //    Name = "Authorization",
                //    Type = "apiKey"
                //}); 
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
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
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(builder =>
                builder.WithOrigins("*"));

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            //app.MapWhen(context => context.Request.Path.Value.Contains("/api"),

            //    appBuilder => appBuilder.UseAPIResponseWrapperMiddleware());

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseAPIResponseWrapperMiddleware();
            });

            //app.UseAPIResponseWrapperMiddleware();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "KNN Null Printer APIs");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.Options.StartupTimeout = new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 30);
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}

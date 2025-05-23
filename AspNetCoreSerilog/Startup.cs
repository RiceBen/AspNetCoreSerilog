using AspNetCoreSerilog.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Linq;

namespace AspNetCoreSerilog
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(
                config =>
                {
                    config.PostProcess = document =>
                    {
                        document.Info.Title = "AspNetCore NSwag";
                        document.Info.Description = "Demo for NSwag on ASP.NET Core";
                        document.Info.Contact = new NSwag.OpenApiContact
                        {
                            Name = "Benjamin Rice",
                            Email = string.Empty,
                            Url = "https://github.com/riceben"
                        };
                        document.Info.License = new NSwag.OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = "https://opensource.org/licenses/mit-license.php"
                        };
                    };

                    var apiScheme = new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT",
                        Description = "JWT Token Auth"
                    };
                    config.AddSecurity("Bearer", Enumerable.Empty<string>(), apiScheme);
                    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                });

            // Add this when you use Jwt Bearer in asp.net core
            services.AddAuthentication().AddJwtBearer();
            services.AddAuthorization();
            // Add MyAuth as a global auth filter
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(MyAuth));
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

            app.UseOpenApi();
            app.UseSwaggerUi();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
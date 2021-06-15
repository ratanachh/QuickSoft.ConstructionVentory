using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using QuickSoft.ConstructionVentory.Infrastructure;
using QuickSoft.ConstructionVentory.Infrastructure.Configurations;
using QuickSoft.ConstructionVentory.Infrastructure.Errors;
using QuickSoft.ConstructionVentory.Infrastructure.Security;

namespace QuickSoft.ConstructionVentory
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DbTransactionPipeLineBehavior<,>));

            
            services.AddLocalization(x => x.ResourcesPath = "Resources");
            
            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {   new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()}
                });
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "QuickSoft API",
                    Version = "v1",
                    Description = "QuickSoft Web API",
                    TermsOfService = new Uri("https://quicksoftteam.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Ratana Chhorm",
                        Email = "ratana@quicksoftteam.com",
                        Url = new Uri("https://mobile.twitter.com/ratana.chhorm"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Proprietary",
                        Url = new Uri("https://quicksoftteam.com/license"),
                    }
                });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => new List<string>()
                {
                    y.GroupName
                });
            });
            services.AddCors();
            
            // Add browser detection service
            services.AddBrowserDetection();

            services.AddMvc(opt =>
                {
                    opt.Conventions.Add(new GroupByApiRootConvention());
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                    opt.EnableEndpointRouting = false;
                })
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.IgnoreNullValues = true; })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            
            services.AddAutoMapper(GetType().Assembly);
            
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            // services.AddScoped<IAuditReader, AuditReader>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilogLogging();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            if (env.IsDevelopment())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                    // c.SerializeAsV2 = true;
                });

                // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickSoft API V1");
                });
                
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthorization();
            
            
            // app.UseAuthentication();
            app.UseMvc();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Index}/{id?}");
            });
        }
    }
}
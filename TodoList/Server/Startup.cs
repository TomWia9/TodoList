using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Linq;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Server.Models;
using TodoList.Server.Repositories;
using Serilog;
using TodoList.Server.Filters;

namespace TodoList.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                 .AddNewtonsoftJson(setupAction =>
                 {
                     setupAction.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                 });
            services.AddRazorPages();
            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;

                options.Filters.Add(new ValidationFilter());
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

                options.OutputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter());
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
                options.ValidatorOptions.LanguageManager.Enabled = false;
            });

            services.AddDbContext<TodoContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("TodoListConnection")));

            services.AddScoped<ITodosRepository, TodosRepository>();
            services.AddScoped<IDbRepository, DbRepository>();
            services.AddScoped<ITodoListsRepository, TodoListsRepository>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "TodoListOpenAPISepcification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "HotelManagement API",
                        Version = "1",
                        //Description = "",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "tomaszwiatrowski9@gmail.com",
                            Name = "Tomasz Wiatrowski",
                            Url = new Uri("https://www.linkedin.com/in/tomasz-wiatrowski-279b00176/")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/TodoListOpenAPISepcification/swagger.json", "TodoList API");
                setupAction.RoutePrefix = "swagger";

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}

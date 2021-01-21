using System;
using System.IO;
using System.Reflection;
using AgendaWebAPI.Data;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AgendaWebAPI
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
            string MySqlConnection = Configuration.GetConnectionString("MySqlConnection");

            services.AddDbContextPool<AgendaContext>(DbContextOptions => DbContextOptions
                   .UseMySql(MySqlConnection, new MySqlServerVersion(new Version(8, 0, 23)),
                   mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend).EnableRetryOnFailure()));

            services.AddScoped<IRepository, Repository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                "v1",
                new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Agenda WebAPI",
                    Version = "v1",
                    TermsOfService = new Uri("http://SeusTermosdeUso.com"),
                    Description = "WebAPI de Agenda com métodos HTTP para Contatos e Eventos",
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "AgendaWebAPI License",
                        Url = new Uri("http://mit.com")
                    },
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Victor Castro",
                        Url = new Uri("https://github.com/victorbfcastro")
                    }
                }
            );
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFullPath);

            }
         );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgendaWebAPI v1"));
            }

            app.UseSwagger()
            .UseSwaggerUI(options =>
            {

                options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");

                options.RoutePrefix = "";
            });
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

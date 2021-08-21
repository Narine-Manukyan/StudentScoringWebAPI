using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ScoringPortal.Data;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StudentScoringWebAPI
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
            services.AddSingleton<StudentData>(new StudentData(Configuration.GetConnectionString("MasterDb"),Configuration.GetConnectionString("StudentScoringPortalDB")));
            services.AddSingleton<TeacherData>(new TeacherData(Configuration.GetConnectionString("MasterDb"), Configuration.GetConnectionString("StudentScoringPortalDB")));
            services.AddSingleton<PrincipalData>(new PrincipalData(Configuration.GetConnectionString("MasterDb"), Configuration.GetConnectionString("StudentScoringPortalDB")));

            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "StudentSpecification",
                    new OpenApiInfo()
                    {
                        Title = "Student",
                        Version = "1",
                        Description = "Student Scoring Portal API (Student)",
                        Contact = new OpenApiContact()
                        {
                            Email = "narine.manukyanprog@gmail.com",
                            Name = "Narine Manukyan"
                        },
                    });
                setupAction.SwaggerDoc(
                    "TeacherSpecification",
                    new OpenApiInfo()
                    {
                        Title = "Teacher",
                        Version = "1",
                        Description = "Student Scoring Portal API (Teacher)",
                        Contact = new OpenApiContact()
                        {
                            Email = "narine.manukyanprog@gmail.com",
                            Name = "Narine Manukyan"
                        },
                    });
                setupAction.SwaggerDoc(
                    "PrincipalSpecification",
                    new OpenApiInfo()
                    {
                        Title = "Principal",
                        Version = "1",
                        Description = "Student Scoring Portal API (Principal)",
                        Contact = new OpenApiContact()
                        {
                            Email = "narine.manukyanprog@gmail.com",
                            Name = "Narine Manukyan"
                        },
                    });

                setupAction.ResolveConflictingActions(apiDescriptions =>
                {
                    return apiDescriptions.First();
                });
                var xmlCommentsFile = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/StudentSpecification/swagger.json",
                    "Student");
                setupAction.SwaggerEndpoint(
                    "/swagger/TeacherSpecification/swagger.json",
                    "Teacher");
                setupAction.SwaggerEndpoint(
                    "/swagger/PrincipalSpecification/swagger.json",
                    "Principal");
            });

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

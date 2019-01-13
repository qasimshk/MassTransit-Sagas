using AutoMapper;
using CQRS.Business;
using CQRS.Business.Messaging;
using CQRS.Commands;
using CQRS.Helper;
using CQRS.Queries;
using CQRS.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CQRS
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
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StudentDatabase")));

            // configuration
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();
            services.AddTransient<IBusConfig>(b => new BusConfiguration(
                "rabbitmq://localhost/", "guest", "guest"));
            

            // repository
            services.AddTransient<IStudentRepository, StudentRepository>();

            // command
            services.AddTransient<ICommandHandler<EditStudentInfoCommand>,
                EditStudentInfoCommandHandler>();
            services.AddTransient<ICommandHandler<UnRegisteredStudentCommand>,
                UnRegisteredStudentCommandHandler>();
            services.AddTransient<ICommandHandler<RegisterStudentInfoCommand>,
                RegisterStudentInfoCommandHandler>();
            services.AddTransient<ICommandPatchHandler<JsonPatchDocument<PartialEditStudentInfoCommand>>,
                PartialEditStudentInfoCommandHandler>();

            // query
            services.AddTransient<IQueryHandler<AllStudentsInfoQuery, object>,
                AllStudentsInfoQueryHandler>();
            services.AddTransient<IQueryHandler<StudentCoursesInfoQuery, object>,
                StudentCoursesInfoQueryHandler>();

            services.AddSingleton<Messages>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWebShop.Common.Extensions;
using MyWebShop.UserApi.Config;
using MyWebShop.UserApi.Models;
using MyWebShop.UserApi.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace MyWebShop.UserApi
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(mvcOptions => {
                mvcOptions.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            /*
            string connectionString = $"Data Source=DESKTOP-E3EL6H1\\SQLEXPRESS;database=ASPNetIdentityCore;trusted_connection=yes;";
            var migrationAssembly = this.GetType().Assembly.GetName().Name;
            services.AddDbContext<IdentityDbContext>(options => {
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly(migrationAssembly));
            });
            services.AddIdentityCore<UserProfile>();
            services.AddScoped<IUserStore<UserProfile>, UserOnlyStore<UserProfile> */
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Example API",
                    Version = "v1"
                });
                c.EnableAnnotations();
            });
            var configSection = services.ReadConfiguration<AppSettings>(this._configuration, "AppSettings");
            services.AddJWTAuthentication(configSection.Get<AppSettings>().Secret);
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI V1");
                c.RoutePrefix = "";
            });
        }
    }
}

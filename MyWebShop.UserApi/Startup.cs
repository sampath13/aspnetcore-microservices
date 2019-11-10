using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyWebShop.UserApi.Config;
using MyWebShop.UserApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

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
            services.AddMvc(mvcOptions =>
            {
                mvcOptions.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            this.AddSwaggerService(services);
            var configSection = this.ReadConfiguration<AppSettings>(services, "AppSettings");
            this.AddJWTAuthentication(services, configSection.Get<AppSettings>().Secret);
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

        private void AddSwaggerService(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Example API",
                    Version = "v1"
                });
                c.EnableAnnotations();
            });
        }

        private IConfigurationSection ReadConfiguration<TOptions>(IServiceCollection services, string sectionName) where TOptions : class
        {
            var appSettingsConfig = this._configuration.GetSection(sectionName);
            services.Configure<TOptions>(appSettingsConfig);
            return appSettingsConfig;
        }

        private void AddJWTAuthentication(IServiceCollection services, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOptions =>
            {
                jwtOptions.RequireHttpsMetadata = false;
                jwtOptions.SaveToken = true;
                jwtOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}

using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChatApp.API.Helpers;
using ChatApp.Const;
using ChatApp.Service;
using ChatApp.Service.App_Start;
using ChatApp.Service.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Buffers;
using System.IO;
using System.Text;

namespace ChatApp.API
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public Startup(IConfiguration configuration, IHostingEnvironment _hostingEnvironment)
        {
            Configuration = configuration;
            hostingEnvironment = _hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

       

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddCors();

            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddMvcOptions(options => { options.OutputFormatters.Add(new PascalCaseJsonProfileFormatter()); });

            #region Identity & JWT
            // ===== Add Identity ========
            services.AddIdentity<AppIdentityUser, AppIdentityRole>(
                config => {
                    config.SignIn.RequireConfirmedEmail = false;
                    config.SignIn.RequireConfirmedPhoneNumber = false;
                    config.Password.RequireDigit = false;
                    config.Password.RequiredUniqueChars = 0;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<WebAPITulparDbContext>()
                .AddDefaultTokenProviders();
             
            services.ConfigureJwtAuthentication();
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });

            #endregion

            #region Database Settings
            if (Messages.RunTimeProjectType == DevelopmentMode.Local)
            {
                services.AddEntityFrameworkNpgsql().AddDbContext<WebAPITulparDbContext>(opt => opt.UseNpgsql(Messages.DatabaseLocalConnectionStr));
            }
            else if (Messages.RunTimeProjectType == DevelopmentMode.Development)
            {
                services.AddEntityFrameworkNpgsql().AddDbContext<WebAPITulparDbContext>(opt => opt.UseNpgsql(Messages.DatabaseTestConnectionStr));
            }
            else if (Messages.RunTimeProjectType == DevelopmentMode.Production)
            {
                services.AddEntityFrameworkNpgsql().AddDbContext<WebAPITulparDbContext>(opt => opt.UseNpgsql(Messages.DatabaseProdConnectionStr));
            }
            #endregion          

            #region Swagger Settigs
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Tulpar Yazılım API", Version = "v1", Description = "This project is the project that developed with ASPCORE Web API 2.2 and powered by Tulpar Yazılım" });
                string xmlPath = Path.Combine(hostingEnvironment.ContentRootPath, "ChatApp.API.xml");
                x.IncludeXmlComments(xmlPath); 
            });
             
            #endregion

            #region Autofac
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new AutofacContext());
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .Where(x => x.Name.EndsWith("Service"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder.RegisterType<EntityUnitofWork<Guid>>().AsSelf().InstancePerLifetimeScope();
            ApplicationContainer = builder.Build();
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(ApplicationContainer);
            #endregion
             
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            { 
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
              
            app.UseStaticFiles();
             
            #region Swagger Settigs
            app.UseSwagger();
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.json", "Tulpar Yazılım API"); });
            #endregion
             
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            
            app.UseMvc();
            app.UseAuthentication();

            AutoMapperConfig<Guid>.Configure();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        }


        public class PascalCaseJsonProfileFormatter : JsonOutputFormatter
        {
            public PascalCaseJsonProfileFormatter() : base(new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() }, ArrayPool<char>.Shared)
            {
                SupportedMediaTypes.Clear();
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json;profile=\"https://en.wikipedia.org/wiki/PascalCase\""));
            }
        }
    }
}

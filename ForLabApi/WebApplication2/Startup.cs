using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ForLabApi.Models;
using ForLabApi.Repositories;
using ForLabApi.DataInterface;
using Microsoft.AspNetCore.Http.Features;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ForLabApi.Utility;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace ForLabApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ForLabContext>(options =>
           options.UseSqlServer(Configuration["Data:DefaultConnection:Connectionstring"]));
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
         
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Appsettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<Appsettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {



                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    //ValidIssuer = "http://localhost:53234",
                    //ValidAudience = "http://localhost:53234",

                    ValidIssuer = "https://forlab-174007.appspot.com",
                    ValidAudience = "https://forlab-174007.appspot.com",

                    //ValidIssuer = "http://forlab.aspwork.co.in/webapi",
                    //ValidAudience = "http://forlab.aspwork.co.in/webapi",



                    IssuerSigningKey = new SymmetricSecurityKey(key),
                
                };
            });
            //EmailSender emailSender = new EmailSender();
            //emailSender.host = Configuration["EmailSender:Host"];
            //emailSender.port = Configuration.GetValue<int>("EmailSender:Port");
            //emailSender.enableSSL = Configuration.GetValue<bool>("EmailSender:EnableSSL");
            //emailSender.userName = Configuration["EmailSender:UserName"];
            //emailSender.password = Configuration["EmailSender:Password"];

            //  services.AddDbContext<HealthCareContext>(options => options.UseSqlServer(connection));
            // app.UseMvc();
            services.AddTransient<IConductforecast<Costclass, ConductforecastDasboard, ConductDashboardchartdata>, ConductforecastAccessRepositories>();
            services.AddTransient<IUserService<User,Importdata,updatepassword, GlobalRegion>, UserAccessRepositories>();
            services.AddTransient<IGroup<Group>, GroupAccessRepositories>();
            services.AddTransient<ISiteCategory<SiteCategory>,DataAccessRepositories>();
            services.AddTransient<ITestingArea<TestingArea>, TestAreaAccessRepositories>();
            services.AddTransient<IProducttype<ProductType>, ProductTypeAccessRepositories>();
            services.AddTransient<IRegion<Region>, RegionAccessRepositories>();
            services.AddTransient<IInstrument<Instrument,InstrumentList,getinstrument, ForecastIns, ForecastInsmodel,forecastinslist>, InstrumentAccessRepositories>();
            services.AddTransient<IForLabSite<Site,sitebyregion,Region,defaultsite, SiteInstrumentList>, SiteAccessRepositories>();
            services.AddTransient<IProduct<MasterProduct, productlist, ProductPrice,ProductType>, ProductAccessRepositories>();
            services.AddTransient<ITest<TestList_area,Gettotalcount, Test, TestingArea,testList,ProductUsagelist, ConsumableUsagelist, Masterconsumablelist, ProductUsageDetail, ConsumableUsageDetail, forecasttest,ForecastTest>,TestAccessRepositories>();
            services.AddTransient<IMMProgram<MMProgramList,demographicMMGeneralAssumption, DemographicMMGroup, DemographicMMGroupList,MMProgram, ForecastInfoList,ForecastInfo, MMForecastParameterList, MMGroupList,MMGeneralAssumptionList,MMGroup, Suggustionlist>, MMProgramAccessRepositories>();
            services.AddTransient<IDashboard<Dashboard, Dashboardchartdata>, DashboardAccessRepositories>();
            services.AddTransient<IForcastInfo<Siteinsvalidation, ForecastInfo, ForecastSiteInfoList, ForecastSiteInfonew,ForecastCategoryInfo, ForecastCategoryInfoList, ForecastCategorySiteInfo,PatientGroup, Test, ForecastCategorySiteInfolist, ForecastInstrumentlist, ForecastProductUsageDetail, ForecastConsumableUsageDetail,forecastusagesmodel>, ForecastInfoAccessRepositories>();
            services.AddTransient<IAssumption<Dynamiccontrol, PatientAssumption, MMGeneralAssumptionValue, TestingAssumption, patientnumberlist, TestingProtocol>, AssumptionAccessRepositories>();
            services.AddTransient<IReport<columnname,Dynamicarray>, ReportAccessRepositories>();
            services.AddTransient<IConsumption<Datausagewithcontrol, siteproductno, adjustedvolume>, ConsumptionAccessRepositories>();
            services.AddTransient<IImport<receivereportdata,Reportobject, ForecastSiteInfonew>, ImportAccessRepositories>();
            services.AddTransient<IApprove<Approve,PendingApprovelist>, Approveaccesssrepositories>();
            services.AddTransient<ICountry<CountryList, MastDiseases, Countrylistusedortrained>, CountryAccessRepositories>();
            services.AddTransient<ICloudStorage,Cloudstoragerepositories>();
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {

               
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
         //       builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); ;
            //  services
            //.AddMvcCore(options => options.OutputFormatters.Add(new XmlSerializerOutputFormatter()));

            //        services.AddCors();
            //       services.AddMvc(options =>
            //       {
            //           options.Filters.Add(new ProducesAttribute("application/xml"));
            //       })
            //.AddXmlSerializerFormatters();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("ApiCorsPolicy");
            app.UseAuthentication();
          //  app.UseMiddleware<CustomMiddleware>();
            app.UseMvc();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.API.Models;
using WebApplication1.Configs;
using WebApplication1.Models;
using WebApplication1.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.API.Auth;
using WebApplication1.API.User;
using Newtonsoft.Json.Linq;
using System.IO;
using WebApplication1.DITest;
using Newtonsoft.Json;

namespace WebApplication1
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
            //services.AddControllersWithViews();

            //���cors ���� ���ÿ�����            
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("any", builder =>
            //    {
            //        builder.AllowAnyOrigin() //�����κ���Դ����������
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();//ָ������cookie
            //    });
            //});

            services.AddCors(options => options.AddPolicy("any",
            builder =>
            {
                builder.AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            //�޸���ͼ����Ҫ����������ˢ��ҳ�漴�ɻ�ȡ�����޸�-----razor�м�� �޸ĺ󼴿�ʵ���Զ�����
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //ע���������ļ��ڵ���Ϣ����Ӧ���ݿ�������Ϣ��
            services.Configure<Database>(Configuration.GetSection("database"));
            string server = Configuration["database:Server"];
            string db = Configuration["database:Name"];
            string uid = Configuration["database:UId"];
            string pwd = Configuration["database:Password"];
            
            string connetion = $"server={server};Database={db};UId={uid};Pwd={pwd}";

            services.AddDbContext<ResultContext>(options=> {
                options.UseSqlServer(connetion);
            });

            services.AddScoped<IResultRepository, ResultRepository>();
            services.AddScoped<IResultTypeRepository, ResultTypeRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();

            //���������ļ�����ע��
            var jsonServices = JObject.Parse(File.ReadAllText("appSettings.json"))["DIServices"];
            var requiredServices = JsonConvert.DeserializeObject<List<Service>>(jsonServices.ToString());

            foreach (var service in requiredServices)
            {
                services.Add(new ServiceDescriptor(serviceType: Type.GetType(service.ServiceType),
                                                   implementationType: Type.GetType(service.ImplementationType),
                                                   lifetime: service.Lifetime));
            }

            //ע��identity��֤
            services.AddIdentity<ResultUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    //options.User.RequireUniqueEmail = false;
                }
                ).AddEntityFrameworkStores<ResultContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                //options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";  //��¼��ַĬ�Ͼ������
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.SlidingExpiration = true;
            });

            //���jwt��֤
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();

            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            //webapi??
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //����Cors
            app.UseCors("any");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers().RequireCors("any");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

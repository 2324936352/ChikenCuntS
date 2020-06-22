using System;
using LPFW.ORM;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using Microsoft.Extensions.Hosting;
using LPFW.WebApplication.Helpers;
using LPFW.ViewModels.MusicViewModel;
using LPFW.EntitiyModels.MusicUIEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace LPFW.WebApplication
{
    public class Startup {
        /// <summary>
        /// APP 启动配置文件：
        /// 1. 配置当前应用系统所需要的所有的 服务；
        /// 2. 定义 HTTP 访问请求处理 管道。
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置服务：
        /// 1.通过代码为当前应用系统容器添加和配置所需要的服务，这里的外部服务意指将用于系统的软件组件，
        ///   例如 EF Core 的 Context 对象就是一个服务。
        /// 2.运行时调用。  
        /// </summary>
        /// <param name="services">注入的服务集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 配置使用 Sql Server 的 EF Context
            services.AddDbContext<LpDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("lpfwDbConnection")));
            // 配置使用微软身份服务组件 Identity Service 的使用
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<LpDbContext>().AddDefaultTokenProviders();

            // 配置 Cookie 使用声明策略
            services.Configure<CookiePolicyOptions>(options => 
            {
                // Cookie 可信说明
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // 配置身份识别的相关策略
            services.Configure<IdentityOptions>(options => 
            {
                // 密码设置
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // 登出策略
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // 用户登录名策略
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                // 缺省登录策略
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });
            // 配置在 App 中 Cookie 的基本限制参数
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie 的一般设置
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // 限制表单上传字节大小
            services.Configure<FormOptions>(options => 
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            // 添加 MVC
            services.AddControllersWithViews(); 
            //services.AddMvc(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));


            //}).AddXmlSerializerFormatters();
            // 添加 IEntityRepository，IViewModelService 等业务数据处理相关的依赖注入元素
            services.BusinessEntityDependencyInjector();

            // 添加定制的控制器过滤器
            services.BusinessFilterConfig();
            //依赖注入
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            services.AddTransient<MusicCore>(); 
        }

        /// <summary>
        /// 配置 HTTP 访问请求处理管道：
        /// 1.通过代码配置已经添加的访问请求处理管道，管道是由一系列的称为 中间件 的组件组成。例如
        ///   处理访问静态文件请求的中间件，将 HTTP 访问重新定向到 HTTPS 的中间件。
        /// 2.每个中间件在一个单一的 HttpContext 执行时，不管是调用管道中的下一个中间件，还是终
        ///   止访问请求，都是异步方式执行操作的。
        /// 3.运行时调用
        /// </summary>
        /// <param name="app">当前运行的 APP 实例。</param>
        /// <param name="env">APP 运行驻留环境。</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();  // 认证用的中间件

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                // 门户路由
                endpoints.MapDefaultControllerRoute();
                // 平台管理区域路由
                endpoints.MapAreaControllerRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                    );
                // 组织机构与人员管理
                endpoints.MapAreaControllerRoute(
                    name: "OrganizationBusinessArea",
                    areaName: "OrganizationBusiness",
                    pattern: "OrganizationBusiness/{controller=Home}/{action=Index}/{id?}"
                    );
                // 课程资源管理
                endpoints.MapAreaControllerRoute(
                    name: "TeachingBusinessArea",
                    areaName: "TeachingBusiness",
                    pattern: "TeachingBusiness/{controller=Home}/{action=Index}/{id?}"
                    );
                // 教师课程内容维护管理
                endpoints.MapAreaControllerRoute(
                    name: "TeacherArea",
                    areaName: "Teacher",
                    pattern: "Teacher/{controller=Home}/{action=Index}/{id?}"
                    );
                // 学生学习管理
                endpoints.MapAreaControllerRoute(
                    name: "StudentArea",
                    areaName: "Student",
                    pattern: "Student/{controller=Home}/{action=Index}/{id?}"
                    );
                // 新闻管理
                endpoints.MapAreaControllerRoute(
                    name: "NewsArea",
                    areaName: "News",
                    pattern: "News/{controller=Home}/{action=Index}/{id?}"
                    );
                // 音乐管理
                endpoints.MapAreaControllerRoute(
                    name: "MusicArea",
                    areaName: "Music",
                    pattern: "Music/{controller=Home}/{action=Index}/{id?}"
                    );
                // 开发时演示示例区域路由
                endpoints.MapAreaControllerRoute(
                    name: "XDemoArea",
                    areaName: "XDemo",
                    pattern: "XDemo/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapAreaControllerRoute(
                   name: "MusicUI",
                   areaName: "MusicUI",
                   pattern: "MusicUI/{controller=Home}/{action=Index}/{id?}"
                   );
             
            });
        }
    }
}

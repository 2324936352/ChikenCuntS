using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LPFW.DataAccess.Seeds;
using LPFW.ORM;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LPFW.WebApplication
{
    public class Program
    {
        /// <summary>
        /// 定义环境接口
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();

        /// <summary>
        /// 程序启动入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // 配置日志
            Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();

            try
            {
                Log.Information("开始启动应用程序");
                
                // 构建 Web 服务实例
                var host = CreateHostBuilder(args).Build();

                // 定义和处理一些系统启动初始化工作
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetService<LpDbContext>();
                    try
                    {
                        // 这里执行相关的种子数据处理代码
                        ApplicationDataSeed.InitialEntity(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "在创建数据种子数据过程中，发生了错误:" + ex.Message);
                    }
                }

                // 启动 Web 服务
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "系统启动失败。");
            }
            finally 
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// 构建 APP 驻留实例的方法：
        /// ASP.Net Core 3.x 使用 WebHost 来创建 WebApp 驻留实例，通过 CreateDefaultBuilder 方法
        /// 使用常规的环境选项（在 Stardup 中配置）设置驻留环境，基本的流程如下：
        ///    1. 使用 Kestrel 作为 Web 服务器以便于 IIS 集成；
        ///    2. 从 appsetting.josn 文件中加载环境变量、命令行参数以及其它资源；
        ///    3. 向控制台等调试器输出日志信息。
        /// </summary>
        /// <param name="args">命令行参数集合</param>
        /// <returns>构建的驻留实例</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => 
            {
                // 配置启动选项（例如限制在系统中上传文件的大小 ...）
                webBuilder.ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = long.MaxValue;
                    options.Limits.MaxRequestBufferSize = long.MaxValue;
                    options.Limits.MaxRequestLineSize = int.MaxValue;
                    //设置应用服务器Kestrel请求体最大为50MB
                    options.Limits.MaxRequestBodySize = 52428800;
                }).UseStartup<Startup>();
                
                // 构建配置
                webBuilder.UseConfiguration(Configuration);

            });
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCore测试1VS2019.Models;

namespace WebCore测试1VS2019
{
    public class Startup
    {

        // 依赖注入注册  （可用于 访问配置信息）
        //     IConfiguration 配置接口
        private readonly IConfiguration _configuration ;
        //     CORS 跨域请求接口名称设置
        private readonly string MyAllowSpecificOrigins = "MyPolicy";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        //builder.WithOrigins("https://localhost:44390", "http://0.0.0.0:3201").AllowAnyHeader();
                        //builder.WithOrigins(urls) // 允许部分站点跨域请求
                        //builder.WithOrigins("https://localhost:8081", "http://192.168.1.105:8081", "http://localhost:2669")
                        builder.WithOrigins("http://localhost:8081", "http://192.168.1.105:8081", "http://localhost:2669")
                                //.AllowAnyOrigin() // 允许所有站点跨域请求（net core2.2版本后将不适用）
                                .SetIsOriginAllowed(t => true) // 允许所有站点跨域请求
                                .AllowAnyMethod() // 允许所有请求方法
                                .AllowAnyHeader() // 允许所有请求头
                                .AllowCredentials(); // 允许Cookie信息
                    });
            });
            services.AddControllers();
            #endregion 跨域

            // 使用DbcontextPool数据库连接池连接数据库（依赖注入）
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDbConnection"))
                );


            // ※步骤1：将所需的MVC服务添加到asp.net core 中的依赖注入容器中。

            /**
             *  A 使用IRouter基本路由逻辑，.NET Core 2.x/3.x 旧版本使用MVC框架方法
             */
            //services.AddMvc(options => options.EnableEndpointRouting = false);

            /**
             *  A 基于Endpoint的路由逻辑，.NET Core 3.x 新版本官方推荐使用MVC框架方法
             */
            services.AddControllersWithViews().AddXmlSerializerFormatters();
            //.AddXmlSerializerFormatters()将请求的数据序列化为xml格式

            // 注册学生管理服务（单例依赖注入）
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            /***
             *      服务类型               同一个Http请求的范围内          横跨多个不同Http请求
             *    Scoped Service                同一个实例                       新实列
             *    Transient Service             新实列                           新实列
             *    Singleton Service             同一个实列                       同一个实列
             * 
             */

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                //developerExceptionPageOptions.SourceCodeLineCount = 10;
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");
            }

            /**
             *  ASP.Net Core 默认不支持 静态文件的服务
             *  默认的静态服务文件夹为 wwwroot
             *  要使用静态文件，必须使用 UseStaticFiles()中间件
             *  要定义默认文件，必须使用 UseDefaultFiles()中间件
             *  默认支持的文件列表： 
             *      Index.html
             *      Index.htm
             *      Default.html
             *      Default.htm
             *  UseDefaultFiles()必须注册在USeStaticFiles()前面
             *  UseFileServer 结合了 UseStaticFiles，UseDefaultFiles 和 UseDirectoryBrowser 中间件的功能
             *  UseDirectoryBrowser() 网页自身浏览器目录预览（暴露项目文件配置）(不推荐)
             */
            /**
            //// 指定默认文件中间件打开的页面
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();  // 清除默认配置
            //defaultFilesOptions.DefaultFileNames.Add("test.html");
            //// 添加默认文件中间件(必须注册在静态文件中间件和UseRouting之前)   // index.html  index.htm 默认  default.html  default.htm
            //app.UseDefaultFiles(defaultFilesOptions);
            */

            // 添加静态文件中间件（少了它将无法访问wwwroot下的文件）
            app.UseStaticFiles();

            /**
            // 配合UseFileServer中间件的配置设置
            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("test.html");
            // 等同于 UseDefailtFiles() + UseStaticFiles() + UseDirectoryBrowser()
            //app.UseFileServer();
            */

            #region 跨域
            app.UseCors(MyAllowSpecificOrigins);
            #endregion

            // 它是用来标记路由决策在请求管道里发生的位置，也就是在这里会选择端点
            app.UseRouting();

            

            /**
             *  .NET Core 3.0
             */
            app.UseAuthorization();

            //  ※步骤2：添加MVC中间件到我们的请求处理管道中。
            /**
             *  B 使用IRouter基本路由逻辑，.NET Core 2.x/3.x 旧版本使用或不用MVC框架方法
             */
            //app.UseMvcWithDefaultRoute();

            // 它是用来标记选择好的端点在请求管道的什么地方来执行
            app.UseEndpoints(endpoints =>
            {
                /**
                 *  B 基于Endpoint的路由逻辑，.NET Core 3.x 新版本官方推荐使用MVC框架方法
                 */
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                /**
                 *  C 使用IRouter基本路由逻辑，.NET Core 2.x/3.x 旧版本使用或不用MVC框架方法
                 */
                //endpoints.MapGet("/", async context =>
                //{
                //    // 声明要响应的内容的类型是utf-8 （防止中文出现乱码，Core对中文的支持极差）
                //    context.Response.ContentType = "text/plain;charset=utf-8";

                //    // 获取当前执行应用程序的进程名称（进程托管）
                //    // InProcess（进程内托管）IIS工作进程 ： iisexpress.exe或w3wp.exe
                //    // OutOfProcess（进程外托管）Kestrel内外两台服务器：dotnet.exe  2.x Core版本显示dotent，3.x Core版本显示当前项目名
                //    var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

                //    // 访问配置信息
                //    //var configVal = _configuration["MyKey"];

                //    //throw new Exception("您的请求在管道中发生了 一些错误，请检查");

                //    await context.Response.WriteAsync("Hello Word !");
                //});

            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sixpence.WeChat.MiniProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sixpence.WeChat
{
    public class Startup : Sixpence.Web.Startup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "接口文档", Version = "v1" });
            });
#endif
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            base.Configure(app, env, accessor);
            app.UseMiniProgramRole();

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"v1/swagger.json", "接口文档");
                c.RoutePrefix = "Swagger";
            });
#endif
        }
    }
}

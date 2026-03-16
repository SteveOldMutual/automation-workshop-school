using Automation_School_API;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_School_API_Tests.Fixtures
{
    using Automation_School_API;
    using Automation_School_API.Context;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System.Runtime.CompilerServices;

    public class TestStartup : Startup
    {
        public IConfiguration Configuration { get; set; }
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Call the base method to register all services
            base.ConfigureServices(services);

           

            // Replace specific services with mocks
            //var mockMyService = new Mock<IMyService>();
            //mockMyService.Setup(s => s.DoSomething()).Returns("Mocked result");

            //// Remove the original service registration
            //var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMyService));
            //if (descriptor != null)
            //{
            //    services.Remove(descriptor);
            //}

            //// Add the mock service
            //services.AddSingleton(mockMyService.Object);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }
}

using Automation_School_API.Context;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_School_API_Tests.Fixtures
{
    public class TestFixture : IDisposable
    {
        private readonly TestServer _server;
        public HttpClient Client { get; }
        public IServiceProvider Services { get; }
        public IMediator Mediator { get; }
        public SchoolDBContext Context { get { return GetService<SchoolDBContext>(); } }

        public TestFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "TEST");
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<TestStartup>();

            _server = new TestServer(webHostBuilder);
            Client = _server.CreateClient();
            Services = _server.Services;
            Mediator = _server.Services.GetRequiredService<IMediator>();
           
        }

        public T GetService<T>()
        {
            return Services.GetRequiredService<T>();
        }

        public async Task InsertAsync<T>(params T[] entities) where T : class
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SchoolDBContext>();
            context.AddRange(entities);
            await context.SaveChangesAsync();
        }
                

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}

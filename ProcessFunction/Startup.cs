using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProcessFunction.Models;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(ProcessFunction.Startup))]

namespace ProcessFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<PersonDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
        }
    }
}

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var keyVaultUrl = new Uri(Environment.GetEnvironmentVariable("KeyVaultUrl"));

            //Connect to Azure keyboard

            var secretClient = new SecretClient(keyVaultUrl, new DefaultAzureCredential());
            var cs = secretClient.GetSecret("sql").Value.Value;
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cs));
        }
    }
}

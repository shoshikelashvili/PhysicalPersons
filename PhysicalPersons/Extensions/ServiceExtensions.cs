using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPersons.Extensions
{
    //Extensions class resposible for configuring service setup methods.
    public static class ServiceExtensions
    {
        //TODO: Once published, CORS setup should be modified to allow requests only from specific origins. 
        public static void ConfigureCors(this IServiceCollection services) =>
         services.AddCors(options =>
         {
             options.AddPolicy("CorsPolicy", builder =>
             builder.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
         });

        //This will be useful during the deployment to IIS servers.
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
         services.Configure<IISOptions>(options =>
         {

         });
    }
}

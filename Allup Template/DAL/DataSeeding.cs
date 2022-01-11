using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Allup_Template.DAL
{
    public static class DataSeeding
    {
        public async static void SeedDatabase(this IApplicationBuilder app)
        {
            using(IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                AppDbContext dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DataInitializer dataInitializer = new DataInitializer(dbcontext);
               await  dataInitializer.InitializeData();
            }
        }
    }
}

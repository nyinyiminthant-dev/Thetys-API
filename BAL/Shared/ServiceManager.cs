using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.IServices;
using BAL.Services;
using Microsoft.Extensions.DependencyInjection;
using MODEL.DTOs;
using MODEL;
using REPOSITORY.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BAL.Shared
{
    public class ServiceManager
    {
        public static void SetServiceInfo(IServiceCollection services, AppSettings appSettings)
        {
            services.AddDbContextPool<DataContext>(options =>
            {
                options.UseSqlServer(appSettings.ConnectionStrings);
            });


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IATMService, ATMService>();
            services.AddScoped<IBankService, BankService>();
        }
    }
}

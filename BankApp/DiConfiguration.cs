using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.BLL;
using BankApp.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp
{
    public static class DiConfiguration
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITransactionService, TransactionService>();
        }
    }
}

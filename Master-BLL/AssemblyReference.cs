using Master_BLL.Services.Implementation;
using Master_BLL.Services.Interface;
using Master_DAL.Abstraction;
using Master_DAL.JWT;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master_BLL
{
    public static class AssemblyReference
    {
        public static IServiceCollection AddBLL(this IServiceCollection services)
        {

            services.AddAuthorization();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            return services;
        }
    }
}

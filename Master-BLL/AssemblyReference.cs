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
            return services;
        }
    }
}

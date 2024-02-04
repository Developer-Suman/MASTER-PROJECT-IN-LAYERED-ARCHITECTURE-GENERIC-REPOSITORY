using Master_DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master_BLL.Services.Interface
{
    public interface IAuthenticationRepository
    {
        Task<ApplicationUser> FindByNameAsync(string username);
        Task<IList<string>> GetRolesAsync(ApplicationUser username);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser username, string password);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> CreateRoles(ApplicationUser user, string role);
    }
}

using Master_BLL.DTOs.RegistrationDTOs;
using Master_BLL.Services.Interface;
using Master_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MASTER_PROJECT_IN_LAYERED_ARCHITECTURE_GENERIC_REPOSITORY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAuthenticationRepository _authenticationRepository;

        public AccountController(IAuthenticationRepository authenticationRepository)
        {
                _authenticationRepository = authenticationRepository;
            
        }

        #region Authentication
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegistrationCreateDTOs registrationCreateDTOs)
        {
            var userExists = await _authenticationRepository.FindByNameAsync(registrationCreateDTOs.Username);
            if(userExists is not null)
            {
                throw new Exception("User Already Exists");
            }

            var emailExists = await _authenticationRepository.FindByEmailAsync(registrationCreateDTOs.Email);
            if(emailExists is not null)
            {
                throw new Exception("Email Alreday Exists");
            }

            ApplicationUser user = new()
            {
                UserName = registrationCreateDTOs.Username,
                Email = registrationCreateDTOs.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _authenticationRepository.CreateUserAsync(user, registrationCreateDTOs.Password);

            if(!result.Succeeded)
            {
                throw new Exception("User Creation Failed");
            }

            return Ok(result);

        }
        #endregion
    }
}

using Master_BLL.DTOs.RegistrationDTOs;
using Master_BLL.Services.Interface;
using Master_DAL.Abstraction;
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
        public async Task<IActionResult> Register(RegistrationCreateDTOs registrationCreateDTOs)
        {
            var userExists = await _authenticationRepository.FindByNameAsync(registrationCreateDTOs.Username);
            if(userExists is not null)
            {
                return BadRequest("User Already Exists");
            }

            var emailExists = await _authenticationRepository.FindByEmailAsync(registrationCreateDTOs.Email);
            if(emailExists is not null)
            {
                return BadRequest("Email Already Exists");
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
                return BadRequest("An error occured while adding user");
            }

            await _authenticationRepository.CreateRoles(user, registrationCreateDTOs.Role);

           
            return Ok(result);



            //public async Task<ActionResult> Register(RegistrationCreateDTOs registrationCreateDTOs)
            //{
            //    // Your registration logic here

            //    // Assume registration is successful and you have some data to return
            //    string registrationResultData = "Registration successful data";

            //    // Create a successful Result using the Success method
            //    Result<string> successResult = Result<string>.Success(registrationResultData);

            //    // Return the appropriate ActionResult based on the Result
            //    if (successResult.IsSuccess)
            //    {
            //        // If registration is successful, return Ok with the data
            //        return Ok(successResult.Data);
            //    }
            //    else
            //    {
            //        // If registration fails, return BadRequest with the errors
            //        return BadRequest(successResult.Errors);
            //    }
            //}

        }
        #endregion
    }
}

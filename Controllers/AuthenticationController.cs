using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.User;
using dotnet_rpg.Model;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            this.authRepo = authRepo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            // Create a service response using the new user data.
            // The user object is built here incase any extra data is to be added by the controller.
            ServiceResponse<int> response = await authRepo.Register(
                new User
                {
                    Username = request.Username
                }, request.Password
            );

            // Respond based on the outcome of the registration call.
            if(!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            // Attempt to login using the auth repository.
            ServiceResponse<string> response = await authRepo.Login(request.Username, request.Password);

            // Respond based on the outcome of the login call.
            if(!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }
    }
}
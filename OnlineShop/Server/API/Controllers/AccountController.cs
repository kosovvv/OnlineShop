using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;

namespace OnlineShop.WebAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            return await this.accountService.GetCurrentUser(User);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return await this.accountService.CheckEmailExistsAsync(email);
        } 

        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<ReturnAddressDto>> GetUserAddress()
        {
            return await this.accountService.GetUserAddress(User);
        }

        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<ReturnAddressDto>> UpdateUserAdresss(ReturnAddressDto address)
        {
            var updatedUser = await this.accountService.UpdateUserAdresss(User, address);

            if (updatedUser == null)
            {
                return BadRequest("Problem updating user");
            }

            return Ok(updatedUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var loggedUser = await this.accountService.Login(loginDto);

            if (loggedUser == null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            return Ok(loggedUser);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await this.accountService.CheckEmailExistsAsync(registerDto.Email))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = ["Email in use"]
                });
            }

            var registeredUser = await this.accountService.Register(registerDto);

            if (registeredUser == null)
            {
                return BadRequest(new ApiResponse(400));
            }
            return Ok(registeredUser);
        }
    }
}
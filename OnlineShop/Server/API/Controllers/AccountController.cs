using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;

namespace OnlineShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ReturnAddressDto>> GetUserAddress()
        {
            return Ok(await this.accountService.GetUserAddress(User));
        }

        [HttpPut("address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<ReturnAddressDto>> UpdateUserAdresss(ReturnAddressDto address)
        {
            try
            {
                var updatedUser = await this.accountService.UpdateUserAdresss(User, address);
                return Ok(updatedUser);
            }
            catch (SavingUserAddressException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                var loggedUser = await this.accountService.Login(loginDto);
                return Ok(loggedUser);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
                throw;
            }
            catch (LoginFailedException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
                throw;
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await this.accountService.CheckEmailExistsAsync(registerDto.Email))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = ["Email in use"]
                });
            }

            try
            {
                var registeredUser = await this.accountService.Register(registerDto);
                return Ok(registeredUser);
            }
            catch (SignUpFailedException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
                throw;
            }
        }
    }
}
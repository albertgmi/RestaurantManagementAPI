using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagementAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto userDto)
        {
            _userService.RegisterUser(userDto);
            return Ok();
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDto dto)
        {
            string token = _userService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}

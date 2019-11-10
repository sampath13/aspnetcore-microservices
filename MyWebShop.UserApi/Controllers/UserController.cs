using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebShop.UserApi.Models;
using MyWebShop.UserApi.Services;

namespace MyWebShop.UserApi.Controllers
{
    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody]User user)
        {
            User loggedInUser = _userService.Authenticate(user.Username, user.Password);
            if (loggedInUser == null) return BadRequest("Invalid login details.");
            return Ok(loggedInUser);
        }

        [HttpGet("all")]
        public IActionResult GetAllUser()
        {
            return Ok(new User[] { new User() { FirstName = "Sampath" }, new User() { FirstName = "Reddy" } });
        }
    }
}
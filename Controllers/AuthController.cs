using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exercise.Data.ModelView;
using Exercise.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
namespace Exercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService){
            _userService = userService;
        }

                // /api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUSerAsync(model);
                if (result.IsSuccess)
                {

                    return Ok(result);  //status code:200
                }

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");// status code:400
        }

        // /api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }


                return Ok(result);
            }

            return BadRequest("Some properties are not valid");
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePaswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                model.Id = userId;
                var result = await _userService.ChangePasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
        
                // /api/auth/DeleteUser
        [Authorize]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.DeleteUser(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }


                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }

        [Authorize]
        [HttpGet("GetMyUser")]
        public async Task<IActionResult> GetMyUser()
        {
            if (ModelState.IsValid)
            {
                var roles = User.IsInRole("Admin");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                var result = await _userService.GetUserbyId(userId);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Some Properties are not Valid");
        }

    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Exercise.Data.Model;
using Exercise.Data.ModelView;
using Exercise.Data.Views;
namespace Exercise.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUSerAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);

        Task<UserManagerResponse> ChangePasswordAsync(ChangePaswordViewModel model);
        Task<UserManagerResponse> DeleteUser(LoginViewModel model);
        Task<Users> GetUserbyId(string UserId);

    }

    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private IConfiguration _configuration;
        public UserService(UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<UserManagerResponse> RegisterUSerAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Register Model is Null");

            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };
            }

            var identityUser = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                DateCreated = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "there is no user with that Email address",
                    IsSuccess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false
                };
            var claims = new List<Claim>
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            if (model.RememberMe)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:Issuer"],
                    audience: _configuration["AuthSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddYears(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                return new UserManagerResponse
                {
                    Message = tokenAsString,
                    IsSuccess = true,
                    ExpireDate = token.ValidTo
                };
            }
            else
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:Issuer"],
                    audience: _configuration["AuthSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                return new UserManagerResponse
                {
                    Message = tokenAsString,
                    IsSuccess = true,
                    ExpireDate = token.ValidTo
                };
            }
        }

        public async Task<UserManagerResponse> ChangePasswordAsync(ChangePaswordViewModel model){
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Cant find that user",
                };

            if (model.NewPassword == model.OldPassword)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "la contraseña debe de ser diferemte a las que has usado.",
                };
            var result = await _userManager.ChangePasswordAsync(user,model.OldPassword,model.NewPassword);
            if (result.Succeeded)
                return new UserManagerResponse
                {
                    Message = "Password has been Changed successfully!",
                    IsSuccess = true,
                };

            return new UserManagerResponse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        public async Task<UserManagerResponse> DeleteUser(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    Message = "User not found",
                    IsSuccess = false,
                };
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false
                };

            var resultDelete = await _userManager.DeleteAsync(user);
            if (resultDelete.Succeeded)
                return new UserManagerResponse
                {
                    Message = "User Deleted successfully!",
                    IsSuccess = true,
                };

            return new UserManagerResponse
            {
                Message = "User did not delete",
                IsSuccess = false,
                Errors = resultDelete.Errors.Select(e => e.Description)
            };

        }

        public async Task<Users> GetUserbyId(string UserId){
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return new Users
                {
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            
            return new Users{
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber??"",
                Id = user.Id,
                ConcurrencyStamp = null,
                Roles = roles,
            };
        }

    }
}
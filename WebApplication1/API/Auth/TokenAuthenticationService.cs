using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.API.Models;
using WebApplication1.API.User;
using WebApplication1.Models;

namespace WebApplication1.API.Auth
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly TokenManagement _tokenManagement;
        public UserManager<ResultUser> UserManager { get; }
        public SignInManager<ResultUser> SignInManager { get; }

        public TokenAuthenticationService(IUserService userService, UserManager<ResultUser> userManager, SignInManager<ResultUser> signInManager, IOptions<TokenManagement> tokenManagement)
        {
            _userService = userService;
            _tokenManagement = tokenManagement.Value;
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public async Task<Dictionary<bool, string>> IsAuthenticated(LoginRequestDTO request)
        {
            var token = string.Empty;
            if (!_userService.IsValid(request))
                return new Dictionary<bool, string> { { false, "" } };
            var resultUser=new ResultUser{ UserName=request.Username };
            //bool result = await UserManager.CheckPasswordAsync(resultUser, request.Password);
            //var result = await SignInManager.CheckPasswordSignInAsync(resultUser, request.Password, false);
            var result = await SignInManager.PasswordSignInAsync(request.Username, request.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.Username)
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);

                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                
                return new Dictionary<bool, string> { { true, token } };
            }
            else
            {
                return new Dictionary<bool, string> { { false, "" } };
            }
            

        }

        public async Task<Dictionary<bool, string>> Register(RegisterRequestDTO request)
        {
            var user = new ResultUser { UserName = request.UserName, Email = request.Email };
            var result = await UserManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new Dictionary<bool, string> { { true, "success" } };
            }
            else
            {
                return new Dictionary<bool, string> { { false, result.Errors.FirstOrDefault().Description } };
            }
            //result.Errors.

        }

        public async Task<ResultUser> GetUserInfoByToken(string token)
        {
            JwtSecurityTokenHandler jwtH = new JwtSecurityTokenHandler();
            if (jwtH.CanReadToken(token))
            {
                JwtSecurityToken jwtToken = jwtH.ReadJwtToken(token);
                string username = jwtToken.Claims.FirstOrDefault().Value;
                var result = await UserManager.FindByNameAsync(username);
                return result;
            }
            return null;
            
            
        }
    }
}

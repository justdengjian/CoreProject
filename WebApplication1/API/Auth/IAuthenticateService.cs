using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.API.Models;
using WebApplication1.Models;

namespace WebApplication1.API.Auth
{
    public interface IAuthenticateService
    {
        //登录
        public Task<Dictionary<bool,string>> IsAuthenticated(LoginRequestDTO request);

        //注册
        public Task<Dictionary<bool, string>> Register(RegisterRequestDTO request);

        public Task<ResultUser> GetUserInfoByToken(string token);
    }
}

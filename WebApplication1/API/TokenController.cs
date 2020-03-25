using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.API.Auth;
using WebApplication1.API.Models;

namespace WebApplication1.API
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("any")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticateService _authService;
        public TokenController(IAuthenticateService authService)
        {
            this._authService = authService;
        }

        /// <summary>
        /// 根据token获取用户信息
        /// </summary>
        /// <param name="GetUserInfoByToken"></param>
        /// <returns></returns>
        [HttpPost, Route("GetUserInfoByToken")]
        
        public async Task<ActionResult> GetUserInfoByToken([FromBody]TokenStr token)
        {

            var resultUser = await _authService.GetUserInfoByToken(token.tokenstr);
            if (resultUser != null)
            {
                return Ok(new { status = 200, msg = "success", data = resultUser });
            }
            else
            {
                return Ok(new { status = 200, msg = "failture", data = "找不到用户信息" });
            }


        }
    }
}
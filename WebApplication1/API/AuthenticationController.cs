using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.API.Auth;
using WebApplication1.API.Models;
using WebApplication1.Models;

namespace WebApplication1.API
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("any")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authService;
        public AuthenticationController(IAuthenticateService authService)
        {
            this._authService = authService;
        }

        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("requestToken")]
        public async Task<ActionResult> RequestToken([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            Dictionary<bool,string> dic = await _authService.IsAuthenticated(request);
            if (dic.ContainsKey(true))
            {
                return Ok(new { status = 200, msg = "success", currentAuthority = dic[true] });
            }
            else if (dic.ContainsKey(false))
            {
                return Ok(new { status = 200, msg = "failure", errormessage = dic[false] });
            }

            return BadRequest("Invalid Request");

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="Register"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            Dictionary<bool, string> dic = await _authService.Register(request);
            if (dic.ContainsKey(true))
            {
                return Ok(new { status = 200, msg = "success" });
            }
            else if (dic.ContainsKey(false))
            {
                return Ok(new { status = 200, msg = "failure", errormessage = dic[false] });
            }

            return BadRequest("Invalid Request");

        }

       






    }
}
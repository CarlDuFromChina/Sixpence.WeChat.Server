using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sixpence.Web.Auth;
using Sixpence.Web.Model;
using Sixpence.Web.Service;
using Sixpence.Web.WebApi;

namespace Sixpence.WeChat.MiniProgram
{
    public class MiniProgramController : BaseApiController
    {
        [HttpGet, HttpPost, AllowAnonymous]
        public string Get()
        {
            return "欢迎使用 Sixpence 小程序接口平台";
        }

        [HttpGet("access_token")]
        public string GetAccessToken()
        {
            return MiniProgramService.AccessToken;
        }

        [HttpGet("login")]
        public LoginResponse Login(string code)
        {
            var request = new LoginRequest()
            {
                third_party_login = new ThirdPartyLogin()
                {
                    param = code,
                    type = "MiniProgram"
                }
            };
            return new SystemService().Login(request);
        }
    }
}


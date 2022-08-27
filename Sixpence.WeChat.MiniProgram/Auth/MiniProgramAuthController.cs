using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sixpence.WeChat.MiniProgram.Model;
using Sixpence.Web.WebApi;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Sixpence.WeChat.MiniProgram.Auth
{
    public class MiniProgramAuthController : BaseApiController
    {
        [HttpGet("openid")]
        public async Task<OpenIdResponse> GetOpenId(string code)
        {
            return await new AuthService().GetOpenId(code);
        }

        [HttpPost("check_encrypted_data")]
        public async Task<bool> CheckEncryptedData([FromBody]dynamic dto)
        {
            return await new AuthService().CheckEncryptedData(dto?.text);
        }

        [HttpGet("phonenumber")]
        public async Task<string> GetPhoneNumber(string code, bool is_pure = true)
        {
            var data = await new AuthService().GetPhoneNumber(code);
            return is_pure ? data.phone_info.purePhoneNumber : data.phone_info.phoneNumber;
        }
    }
}


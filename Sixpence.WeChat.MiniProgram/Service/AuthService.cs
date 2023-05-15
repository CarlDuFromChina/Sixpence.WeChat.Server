using System;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Sixpence.Common.Logging;
using Sixpence.Common.Utils;
using Sixpence.WeChat.Common.Util;
using Sixpence.WeChat.MiniProgram.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Sixpence.WeChat.MiniProgram.Service
{
    public class AuthService
    {
        private ILog logger { get; set; }
        private object lockObj = new object();

        public AuthService()
        {
            logger = LoggerFactory.GetLogger(GetType().Name);
        }

        /// <summary>
        /// 小程序登录
        /// </summary>
        /// <param name="code"></param>
        /// <see cref="https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/login.html"/>
        /// <returns></returns>
        public LoginResponseModel Login(string code)
        {
            var url = MiniProgramApiConfig.GetValue("Login");
            var appid = MiniProgramConfig.Config.AppId;
            var secret = MiniProgramConfig.Config.AppSecret;
            logger.Debug("微信登录请求地址：" + string.Format(url, appid, secret, code));
            var resp = HttpUtil.Get(string.Format(url, appid, secret, code));
            logger.Debug("微信登录返回值：" + resp);
            var data = JsonConvert.DeserializeObject<LoginResponseModel>(resp);
            WxAssertUtil.CheckWXResponse(data);
            lock (lockObj)
            {
                if (!MiniProgramService.Session.TryGetValue(data.openid, out var value))
                {
                    MiniProgramService.Session.Add(data.openid, data);
                }
            }
            return data;
        }

        /// <summary>
        /// 获取用户openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OpenIdResponse> GetOpenId(string code)
        {
            var url = MiniProgramApiConfig.GetValue("GetOpenId");
            url = string.Format(url, MiniProgramService.AccessToken);
            var param = new { code };
            logger.Debug("GetOpenId 请求地址：" + url + "，入参：" + JsonConvert.SerializeObject(param));
            var resp = await HttpUtil.PostAsync(url, JsonConvert.SerializeObject(param));
            logger.Debug("GetOpenId 返回值：" + resp);
            return JsonConvert.DeserializeObject<OpenIdResponse>(resp);
        }

        /// <summary>
        /// 检查加密信息是否由微信生成（当前只支持手机号加密数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<bool> CheckEncryptedData(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            var url = MiniProgramApiConfig.GetValue("CheckEncryptedData");
            url = string.Format(url, MiniProgramService.AccessToken);
            var param = new { encrypted_msg_hash = text };
            logger.Debug("CheckEncryptedData 请求地址：" + url + "，入参：" + JsonConvert.SerializeObject(param));
            var resp = await HttpUtil.PostAsync(url, JsonConvert.SerializeObject(param));
            logger.Debug("CheckEncryptedData 返回值：" + resp);
            var data = JsonConvert.DeserializeObject<CheckEncryptedDataResponse>(resp);
            WxAssertUtil.CheckWXResponse(data);
            return data.vaild;
        }

        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<PhoneNumberResponse> GetPhoneNumber(string code)
        {
            var url = MiniProgramApiConfig.GetValue("GetPhoneNumber");
            url = string.Format(url, MiniProgramService.AccessToken);
            var param = new { code };
            logger.Debug("GetPhoneNumber 请求地址：" + url + "，入参：" + JsonConvert.SerializeObject(param));
            var resp = await HttpUtil.PostAsync(url, JsonConvert.SerializeObject(param));
            logger.Debug("GetPhoneNumber 返回值：" + resp);
            var data = JsonConvert.DeserializeObject<PhoneNumberResponse>(resp);
            WxAssertUtil.CheckWXResponse(data);
            return data;
        }
    }
}


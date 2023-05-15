using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sixpence.Common.Utils;
using Sixpence.WeChat.Common.Model;
using Sixpence.WeChat.MiniProgram.Model;

namespace Sixpence.WeChat.MiniProgram.Service
{
    public class MiniProgramService
    {
        public const string ACESS_TOEKN_NAME = "MiniPorgram_AccessToken";
        private static readonly object lockObject = new object();
        public static Dictionary<string, LoginResponseModel> Session = new Dictionary<string, LoginResponseModel>();

        /// <summary>
        /// 获取access_token
        /// </summary>
        public static string AccessToken
        {
            get
            {
                var accessToken = MemoryCacheUtil.GetCacheItem<AccessTokenResponse>(ACESS_TOEKN_NAME);
                if (accessToken == null)
                {
                    lock (lockObject)
                    {
                        if (accessToken == null)
                        {
                            accessToken = RefreshToken();
                        }
                    }
                }
                return accessToken.AccessToken;
            }
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public static AccessTokenResponse RefreshToken()
        {
            var accessToken = GetAccessToken();
            MemoryCacheUtil.RemoveCacheItem(ACESS_TOEKN_NAME);
            MemoryCacheUtil.Set(ACESS_TOEKN_NAME, accessToken);
            return accessToken;
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        public static AccessTokenResponse GetAccessToken()
        {
            var url = MiniProgramApiConfig.GetValue("GetAccessTokenApi");
            var appid = MiniProgramConfig.Config.AppId;
            var secret = MiniProgramConfig.Config.AppSecret;
            url = string.Format(url, appid, secret);
            var response = HttpUtil.Get(url);
            var respJson = JObject.Parse(response);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "获取微信授权失败");
            return new AccessTokenResponse()
            {
                AccessToken = respJson.GetValue("access_token").ToString(),
                Expire = Convert.ToInt32(respJson.GetValue("expires_in").ToString())
            };
        }
    }
}


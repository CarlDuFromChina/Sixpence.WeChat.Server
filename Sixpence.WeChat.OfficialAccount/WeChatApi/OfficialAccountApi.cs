using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sixpence.Common;
using Sixpence.Common.Utils;
using Sixpence.WeChat.Common.Model;
using Sixpence.WeChat.OfficialAccount.Model;
using System.IO;
using Sixpence.WeChat.OfficialAccount.Service;
using Sixpence.WeChat.OfficialAccount.Extension;

namespace Sixpence.WeChat.OfficialAccount.WeChatApi
{
    public partial class OfficialAccountApi
    {
        public static readonly string BaseUrl = OfficialAccountApiConfig.GetValue("Url");

        /// <summary>
        /// 获取微信Token
        /// </summary>
        public static (string AccessToken, int Expire) GetAccessToken(string appid, string secret)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("GetAccessTokenApi"), appid, secret);
            var resp = HttpUtil.Get(url);
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "获取微信授权失败");
            return (respJson.GetValue("access_token").ToString(), Convert.ToInt32(respJson.GetValue("expires_in").ToString()));
        }

        public static void CheckWeChatErrorResponse(JObject respJson, string message)
        {
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", message);
        }
    }
}


using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Sixpence.Common.Utils;
using Sixpence.WeChat.OfficialAccount.Model;
using Sixpence.WeChat.OfficialAccount.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.WeChat.OfficialAccount.WeChatApi
{
    public partial class OfficialAccountApi
    {
        public static WeChatIndustryResponse GetIndustry()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetIndustry"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "获取行业信息失败");
            return JsonConvert.DeserializeObject<WeChatIndustryResponse>(resp);
        }

        public static WeChatTemplateResponse GetTemplate()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetTemplate"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "获取模板失败");
            return JsonConvert.DeserializeObject<WeChatTemplateResponse>(resp);
        }

        public static void DeleteTemplate(string templateId)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("DeletetTemplate"), OfficialAccountService.AccessToken);
            var postData = new
            {
                template_id = templateId
            };
            var resp = HttpUtil.Post(url, JsonConvert.SerializeObject(postData));
            CheckWeChatErrorResponse(JObject.Parse(resp), "删除模板失败");
        }

        public static void SendTemplateMessage(string openid, string templateid, string data, string url = "", string appid = "", string pagepath = "")
        {
            var _url = string.Format(OfficialAccountApiConfig.GetValue("SendTemplateMessage"), OfficialAccountService.AccessToken);
            var postData = new
            {
                touser = openid,
                template_id = templateid,
                url,
                miniprogram = new
                {
                    appid,
                    pagepath
                },
                client_msg_id = openid + Guid.NewGuid().ToString(),
                data
            };
            var resp = HttpUtil.Post(_url, JsonConvert.SerializeObject(postData));
            CheckWeChatErrorResponse(JObject.Parse(resp), "删除模板失败");
        }
    }
}

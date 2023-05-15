using Newtonsoft.Json.Linq;
using Sixpence.Common.Utils;
using Sixpence.WeChat.OfficialAccount.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.WeChat.OfficialAccount.WeChatApi
{
    /// <summary>
    /// 公众号用户 API
    /// </summary>
    public partial class OfficialAccountApi
    {
        /// <summary>
        /// 获取关注用户列表
        /// <para>next_openid（当公众号关注者数量超过10000时，可通过填写next_openid的值，从而多次拉取列表的方式来满足需求。）</para>
        /// </summary>
        public static string GetFocusUserList(string nextOpenId = "")
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetFocusUserListApi"), OfficialAccountService.AccessToken, nextOpenId));
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "获取关注用户列表失败");
            return resp;
        }

        /// <summary>
        /// 获取关注用户基本信息
        /// </summary>
        public static string GetFocusUser(string openId, string lang = "zh_CN")
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetFocusUserApi"), OfficialAccountService.AccessToken, openId, lang));
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "获取关注用户基本信息失败");
            return resp;
        }

        /// <summary>
        /// 批量获取关注用户基本信息
        /// </summary>
        public static string BatchGetFocusUser(string postData)
        {
            var resp = HttpUtil.Post(string.Format(OfficialAccountApiConfig.GetValue("BatchGetFocusUserApi"), OfficialAccountService.AccessToken), postData);
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "批量获取关注用户基本信息失败");
            return resp;
        }
    }
}

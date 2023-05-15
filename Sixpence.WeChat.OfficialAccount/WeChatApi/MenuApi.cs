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
    /// 公众号菜单 API
    /// </summary>
    public partial class OfficialAccountApi
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        public static string CreateMenu(string postData)
        {
            var resp = HttpUtil.Post(string.Format(OfficialAccountApiConfig.GetValue("CreateMenuApi"), OfficialAccountService.AccessToken), postData);
            CheckWeChatErrorResponse(JObject.Parse(resp), "创建菜单失败");
            return resp;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public static string GetMenus()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetMenuApi"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "获取微信菜单失败");
            return resp;
        }

        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <param name="postData"></param>
        public static void DeleteMenu()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("DeleteMenuApi"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "删除微信菜单失败");
        }
    }
}

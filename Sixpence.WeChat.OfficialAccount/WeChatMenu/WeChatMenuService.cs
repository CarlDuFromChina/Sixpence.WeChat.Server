using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Sixpence.WeChat.OfficialAccount.WeChatMenu
{
    public static class WeChatMenuService
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menu"></param>
        public static void CreateMenu(WeChatCreateMenuModel menu)
        {
            OfficialAccountApi.CreateMenu(JsonConvert.SerializeObject(menu));
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <returns></returns>
        public static WeChatMenuModel GetMenus()
        {
            var resp = OfficialAccountApi.GetMenus();
            return JsonConvert.DeserializeObject<WeChatMenuModel>(resp);
        }

        /// <summary>
        /// 删除全部菜单
        /// </summary>
        public static void DeleteMenu()
        {
            OfficialAccountApi.DeleteMenu();
        }
    }
}

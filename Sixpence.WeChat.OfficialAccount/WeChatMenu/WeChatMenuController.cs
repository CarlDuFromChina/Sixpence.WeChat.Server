using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.WeChat.OfficialAccount.WeChatMenu
{
    public class WechatMenuController : BaseApiController
    {
        [HttpPost]
        public void CreateMenu(WeChatCreateMenuModel menu)
        {
            WeChatMenuService.CreateMenu(menu);
        }

        [HttpGet]
        public WeChatMenuModel GetMenus()
        {
            return WeChatMenuService.GetMenus();
        }

        [HttpDelete]
        public void DeleteMenu()
        {
            WeChatMenuService.DeleteMenu();
        }
    }
}

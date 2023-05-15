using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.WeChat.OfficialAccount.Service;
using Sixpence.WeChat.OfficialAccount.Model;

namespace Sixpence.WeChat.OfficialAccount.Controller
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

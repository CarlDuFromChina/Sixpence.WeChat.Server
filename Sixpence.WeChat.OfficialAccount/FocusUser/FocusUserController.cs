using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.WeChat.OfficialAccount.FocusUser
{
    public class FocusUserController : EntityBaseController<focus_user, FocusUserService>
    {
        [HttpGet("focus_user")]
        public FocusUserListModel GetFocusUserList()
        {
            return new FocusUserService().GetFocusUserList();
        }
    }
}

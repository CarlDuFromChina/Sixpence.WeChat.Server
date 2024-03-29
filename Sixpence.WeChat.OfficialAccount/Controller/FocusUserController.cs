﻿using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.WeChat.OfficialAccount.Model;
using Sixpence.WeChat.OfficialAccount.Service;
using Sixpence.WeChat.OfficialAccount.Entity;

namespace Sixpence.WeChat.OfficialAccount.Controller
{
    public class FocusUserController : EntityBaseController<FocusUser, FocusUserService>
    {
        [HttpGet("focus_user")]
        public FocusUserListModel GetFocusUserList()
        {
            return new FocusUserService().GetFocusUserList();
        }
    }
}

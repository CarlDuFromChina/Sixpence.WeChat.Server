﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Auth;
using Sixpence.Web.Auth.Role;
using Sixpence.Web.Auth.Role.BasicRole;
using Sixpence.Web.Entity;

namespace Sixpence.WeChat.MiniProgram.Extension
{
    public static class MiniProgramUserSetupExtension
    {
        public static IApplicationBuilder UseMiniProgramRole(this IApplicationBuilder app)
        {
            var manager = EntityManagerFactory.GetManager();
            if (manager.QueryFirst<SysRole>("444444444-44444-4444-4444-444444444444") == null)
            {
                var role = new SysRole()
                {
                    id = "444444444-44444-4444-4444-444444444444",
                    name = "微信小程序角色",
                    is_basic = false,
                    description = "微信小程序专属角色",
                    parent_roleid = UserIdentityUtil.USER_ID,
                    parent_roleid_name = "普通用户",
                };
                manager.Create(role);

            }
            return app;
        }
    }
}


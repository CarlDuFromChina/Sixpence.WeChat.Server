using System;
using System.Xml.Linq;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Auth;
using Sixpence.Web.Auth.Role;
using Sixpence.Web.Config;
using Sixpence.Web.Entity;
using Sixpence.WeChat.MiniProgram.Entity;

namespace Sixpence.WeChat.MiniProgram.Plugin
{
    public class MpUserPlugin : IEntityManagerPlugin
    {
        public void Execute(EntityManagerPluginContext context)
        {
            var entity = context.Entity as MpUser;
            switch (context.Action)
            {
                case EntityAction.PostCreate:
                    CreateUserInfo(context.EntityManager, entity);
                    break;
                default:
                    break;
            }
        }

        public void CreateUserInfo(IEntityManager manager, MpUser mpUser)
        {
            UserInfo user = manager.QueryFirst<UserInfo>(mpUser.id);
            if (user == null)
            {
                user = new UserInfo()
                {
                    id = mpUser.openid,
                    code = mpUser.openid,
                    password = SystemConfig.Config.DefaultPassword,
                    name = mpUser.nickname,
                    roleid = "444444444-44444-4444-4444-444444444444",
                    roleid_name = "微信小程序角色",
                    statecode = true,
                    statecode_name = "启用"
                };

                manager.Create(user);
            }
        }
    }
}


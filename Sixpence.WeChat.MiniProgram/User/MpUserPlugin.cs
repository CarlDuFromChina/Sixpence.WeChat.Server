using System;
using System.Xml.Linq;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Auth;
using Sixpence.Web.Auth.Role;
using Sixpence.Web.Auth.UserInfo;
using Sixpence.Web.Config;

namespace Sixpence.WeChat.MiniProgram.User
{
    public class MpUserPlugin : IEntityManagerPlugin
    {
        public void Execute(EntityManagerPluginContext context)
        {
            var entity = context.Entity as mp_user;
            switch (context.Action)
            {
                case EntityAction.PostCreate:
                    CreateUserInfo(context.EntityManager, entity);
                    break;
                default:
                    break;
            }
        }

        public void CreateUserInfo(IEntityManager manager, mp_user mpUser)
        {
            user_info user = manager.QueryFirst<user_info>(mpUser.id);
            if (user == null)
            {
                user = new user_info()
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


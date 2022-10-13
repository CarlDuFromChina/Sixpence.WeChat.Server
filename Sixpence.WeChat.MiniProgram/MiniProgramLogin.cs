using System;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Auth;
using Sixpence.Web.Auth.Role.BasicRole;
using Sixpence.WeChat.MiniProgram.Auth;
using Sixpence.WeChat.MiniProgram.User;

namespace Sixpence.WeChat.MiniProgram
{
    public class MiniProgramLogin : IThirdPartyLoginStrategy
    {
        private IEntityManager manager = EntityManagerFactory.GetManager();

        public string GetName() => "MiniProgram";

        public LoginResponse Login(object param)
        {
            var code = param as string;
            var wechatUser = new AuthService().Login(code); // 微信登录数据
            var openid = wechatUser.openid;

            return manager.ExecuteTransaction(() =>
            {
                UserIdentityUtil.SetCurrentUser(UserIdentityUtil.GetSystem());
                var data = new MpUserService(manager).GetData(openid);
                if (data == null)
                {
                    data = new mp_user()
                    {
                        id = openid,
                        openid = openid,
                    };
                    manager.Create(data);
                }
                var auth = manager.QueryFirst<auth_user>(openid);
                return new LoginResponse()
                {
                    result = true,
                    userName = code,
                    token = JwtHelper.CreateToken(new JwtTokenModel() { Code = auth.code, Name = $"微信用户{openid}", Role = auth.roleid, Uid = auth.id }),
                    userId = wechatUser.openid,
                    message = "登录成功"
                };
            });
        }
    }
}


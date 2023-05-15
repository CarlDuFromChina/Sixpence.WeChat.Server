using System;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Service;

namespace Sixpence.WeChat.MiniProgram.User
{
    public class MpUserService : EntityService<MpUser>
    {
        public MpUserService() : base() { }

        public MpUserService(IEntityManager manager) : base(manager) { }
    }
}


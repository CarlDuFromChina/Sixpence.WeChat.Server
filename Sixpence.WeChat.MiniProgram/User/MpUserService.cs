using System;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;

namespace Sixpence.WeChat.MiniProgram.User
{
    public class MpUserService : EntityService<mp_user>
    {
        public MpUserService() : base() { }

        public MpUserService(IEntityManager manager) : base(manager) { }
    }
}


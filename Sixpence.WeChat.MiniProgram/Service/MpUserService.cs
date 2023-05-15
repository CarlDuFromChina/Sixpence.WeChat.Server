using System;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Service;
using Sixpence.WeChat.MiniProgram.Entity;

namespace Sixpence.WeChat.MiniProgram.Service
{
    public class MpUserService : EntityService<MpUser>
    {
        public MpUserService() : base() { }

        public MpUserService(IEntityManager manager) : base(manager) { }
    }
}


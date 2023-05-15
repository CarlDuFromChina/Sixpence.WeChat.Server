using System;
using Quartz;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Job;
using Sixpence.WeChat.OfficialAccount.Service;

namespace Sixpence.WeChat.OfficialAccount.Job
{
    public class SyncFocusUserJob : JobBase
    {
        public override string Name => "同步微信公众号关注用户";

        public override string Description => "同步微信公众号关注用户";

        public override IScheduleBuilder ScheduleBuilder => CronScheduleBuilder.CronSchedule("0 0 4 * * ?");

        public override TriggerState DefaultTriggerState => TriggerState.Paused;

        public override void Executing(IJobExecutionContext context)
        {
            var Manager = EntityManagerFactory.GetManager();
            Logger.Debug("开始同步微信公众号关注用户");
            Manager.ExecuteTransaction(() =>
            {
                var service = new FocusUserService(Manager);
                var list = service.GetFocusUserList();
                if (list.count > 0)
                {
                    list.data.openid.ForEach(id =>
                    {
                        service.SaveData(id);
                    });
                }
            });
        }
    }
}


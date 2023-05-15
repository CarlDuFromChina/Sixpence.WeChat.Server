using Quartz;
using Sixpence.Common;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Auth;
using Sixpence.Web.Job;
using Sixpence.WeChat.Entity;
using Sixpence.WeChat.OfficialAccount.Entity;
using System.Linq;

namespace Sixpence.WeChat.Job
{
    public class HolidayJob : JobBase
    {
        public override string Name => "纪念日消息推送";

        public override string Description => "纪念日消息推送";

        public override IScheduleBuilder ScheduleBuilder => CronScheduleBuilder.CronSchedule("0 0 0 8 * ? ");

        public override void Executing(IJobExecutionContext context)
        {
            var manager = EntityManagerFactory.GetManager();
            var users = manager.Query<FocusUser>("select * from focus_user");
            var holidays = manager.Query<Holiday>("select * from holiday");
            var innerUsers = new string[]
            {
                UserIdentityUtil.ADMIN_ID,
                UserIdentityUtil.SYSTEM_ID,
                UserIdentityUtil.UserId,
                UserIdentityUtil.ANONYMOUS_ID
            };

            var dataList = from user in users
                           join holiday in holidays on user.id equals holiday.created_by into mn
                           select new
                           {
                               user,
                               holidays = mn.ToList()
                           };

            dataList.Each(data =>
            {
                // 公众号消息推送

            });
        }
    }
}

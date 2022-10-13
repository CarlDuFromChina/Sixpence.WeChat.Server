using Sixpence.ORM.Entity;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sixpence.WeChat.UserSetting
{
    [Entity]
    public class user_setting : BaseEntity
    {
        [DataMember]
        [PrimaryColumn]
        public string id { get; set; }

        /// <summary>
        /// 重复次数：不重复、1、2、3、4....
        /// </summary>
        [DataMember, Column, Description("重复次数")]
        public string notify_repeat_time { get; set; }

        /// <summary>
        /// 重复类型：不重复、天、月、年
        /// </summary>
        [DataMember, Column, Description("重复类型")]
        public string notify_repeat_unit { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        [DataMember, Column, Description("通知类型")]
        public string notify_type { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember, Column, Description("用户id")]
        public string userid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, Column, Description("用户名")]
        public string username { get; set; }
    }
}

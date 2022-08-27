using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sixpence.ORM.Entity;

namespace Sixpence.WeChat.MiniProgram.User
{
    [Entity("mp_user", "微信小程序用户")]
    public class mp_user : BaseEntity
    {
        [PrimaryColumn, DataMember]
        public string id { get; set; }

        [Column, DataMember]
        public string openid { get; set; }

        [Column, DataMember]
        public string unionid { get; set; }

        [Column, DataMember, Description("昵称")]
        public string nickname { get; set; }

        [Column, DataMember, Description("头像地址")]

        public string avatarurl { get; set; }

        [Column, DataMember, Description("语言")]
        public string lang { get; set; }

        [Column, DataMember, Description("描述"), JsonPropertyName("desc")]
        public string description { get; set; }

        /// <summary>
        /// //性别 0：未知、1：男、2：女
        /// </summary>
        [Column, DataMember, Description("性别")]
        public int? gender { get; set; }

        [Column, DataMember, Description("省份")]
        public string province { get; set; }

        [Column, DataMember, Description("城市")]
        public string city { get; set; }

        [Column, DataMember, Description("区")]
        public string country { get; set; }
    }
}


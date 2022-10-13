using Sixpence.ORM.Entity;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sixpence.WeChat.Holiday
{
    [Entity("holiday")]
    [KeyAttributes("纪念日名称不能重复", "created_by", "name")]
    public class holiday : BaseEntity
    {
        [DataMember]
        [PrimaryColumn]
        public string id { get; set; }

        [DataMember, Column, Description("名称")]
        public string name { get; set; }

        [DataMember, Column, Description("目标日")]
        public  DateTime? target_datetime { get; set; }

        [DataMember, Column, Description("分类")]
        public string classification { get; set; }
    }
}

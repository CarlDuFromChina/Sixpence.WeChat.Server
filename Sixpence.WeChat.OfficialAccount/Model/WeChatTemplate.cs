using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.WeChat.OfficialAccount.Model
{
    public class WeChatTemplate
    {
        public string template_id { get;set; }
        public string title { get;set; }
        public string primary_industry { get; set; }
        public string deputy_industry { get; set; }
        public string content { get; set; }
        public string example { get; set; }
    }

    public class WeChatIndustryResponse
    {
        public WeChatIndustry primary_industry { get; set; }
        public WeChatIndustry secondary_industry { get; set; }
    }

    public class WeChatIndustry
    {
        public string first_class { get; set; } 
        public string second_class { get; set; }
    }

    public class WeChatTemplateResponse
    {
        public List<WeChatTemplate> template_list { get; set; }
    }
}

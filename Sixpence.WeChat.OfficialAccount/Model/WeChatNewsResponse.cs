using System;
using System.Collections.Generic;

namespace Sixpence.WeChat.OfficialAccount.Model
{

    public class WeChatNewsResponse
    {
        public int total_count { get; set; }
        public int item_count { get; set; }
        public List<WeChatNewsItem> item { get; set; }
    }

    public class WeChatNewsItem
    {
        public string media_id { get; set; }
        public WeChatNewsContent content { get; set; }
        public long update_time { get; set; }
    }

    public class WeChatNewsContent
    {
        public List<WeChatNewsItems> news_item { get; set; }
    }

    public class WeChatNewsItems
    {
        public string title { get; set; }
        public string thumb_media_id { get; set; }
        public int show_cover_pic { get; set; }
        public string author { get; set; }
        public string digest { get; set; }
        public string content { get; set; }
        public string url { get; set; }
        public string content_source_url { get; set; }
    }

    public class WeChatAddNewsResponse
    {
        public string media_id { get; set; }
    }
}


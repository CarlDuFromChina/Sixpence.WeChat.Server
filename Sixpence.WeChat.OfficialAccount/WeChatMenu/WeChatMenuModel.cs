using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.WeChat.OfficialAccount.WeChatMenu
{
    public class WeChatMenuModel
    {
        /// <summary>
        /// 菜单是否开启，0代表未开启，1代表开启
        /// </summary>
        public int is_menu_open { get; set; }

        /// <summary>
        /// 菜单信息
        /// </summary>
        public SelfMenuInfo selfmenu_info { get; set; }
    }

    public class SelfMenuInfo
    {
        /// <summary>
        /// 菜单按钮
        /// </summary>
        public List<WeChatMenuButtonModel> button { get; set; }
    }

    public class WeChatMenuSubButtonModel
    {
        public List<WeChatMenuButtonModel> list { get; set; }
    }

    public class WeChatMenuButtonModel
    {
        /// <summary>
        /// 菜单的类型，公众平台官网上能够设置的菜单类型有view（跳转网页）、text（返回文本，下同）、img、photo、video、voice。使用 API 设置的则有8种，详见《自定义菜单创建接口》
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///类型为click必须要key
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// View：保存链接到url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Text:保存文字到value； Img、voice：保存 mediaID 到value；Video：保存视频下载链接到value； News：保存图文消息到news_info，同时保存 mediaID 到value；
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public WeChatMenuSubButtonModel sub_button { get; set; }

        /// <summary>
        /// 图文消息的信息
        /// </summary>
        public NewsInfo news_info { get; set; }
    }

    public class NewsInfo
    {
        public List<News> list { get; set; }
    }

    public class News
    {
        public string title { get; set; }
        public string author { get; set; }
        public string digest { get; set; }
        public int show_cover { get; set; }
        public string cover_url { get; set; }
        public string content_url { get; set; }
        public string source_url { get; set; }
    }

    public class WeChatCreateMenuModel
    {
        public List<WeChatCreateMenuButtonModel> button { get; set; }
    }

    public class WeChatCreateMenuButtonModel
    {
        public string name { get; set; }
        public List<WeChatCreateMenuButtonModel> sub_button { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public string media_id { get; set; }
        public string appid { get; set; }
        public string pagepath { get; set; }
        public string article_id { get; set; }
    }
}

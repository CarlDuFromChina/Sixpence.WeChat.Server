using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sixpence.WeChat.OfficialAccount.Model
{
    /// <summary>
    /// 素材类型枚举
    /// </summary>
    public enum MaterialType
    {
        [Description("图片")]
        image,
        [Description("视频")]
        video,
        [Description("语音")]
        voice,
        [Description("图文")]
        news
    }
    public class BaseWeChatMaterial<T>
        where T : BaseWeChatMaterialItem, new()
    {
        public int total_count { get; set; }

        public int item_count { get; set; }

        public List<T> item { get; set; }
    }

    /// <summary>
    /// 微信图文素材
    /// </summary>
    public class WeChatNewsl : BaseWeChatMaterial<WeChatNewsMaterialItem> { }

    /// <summary>
    /// 微信其他素材
    /// </summary>
    public class WeChatOtherMaterial : BaseWeChatMaterial<MediaMaterial> { }

    /// <summary>
    /// 微信素材内容基类
    /// </summary>
    public class BaseWeChatMaterialItem
    {
        /// <summary>
        /// 媒体Id
        /// </summary>
        public string media_id { get; set; }

        /// <summary>
        /// 更新时间（新增格式化后时间）
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long update_time { get; set; }
    }

    #region 图文消息素材

    /// <summary>
    /// 图文消息
    /// </summary>
    public class WeChatNewsMaterialItem : BaseWeChatMaterialItem
    {
        public NewsContent content { get; set; }
    }

    public class NewsContent
    {
        public List<NewsItem> news_item { get; set; }
    }

    public class NewsItem
    {
        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string digest { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
    }
    #endregion

    #region 其他素材
    /// <summary>
    /// 其他类型（图片、语音、视频）
    /// </summary>
    public class MediaMaterial : BaseWeChatMaterialItem
    {
        public string name { get; set; }

        public string url { get; set; }
    }
    #endregion

}


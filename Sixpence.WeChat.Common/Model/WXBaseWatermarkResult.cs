using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Sixpence.WeChat.Common.Model
{
    public class WXBaseWatermarkResult
    {
        /// <summary>
        /// 水印
        /// </summary>
        public WXWatermark Watermark { get; set; }
    }

    /// <summary>
    /// 微信水印
    /// </summary>
    public class WXWatermark
    {
        /// <summary>
        /// AppID
        /// </summary>
        public string Appid { get; set; }

        /// <summary>
        /// 时间戳（Unix时间）
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}


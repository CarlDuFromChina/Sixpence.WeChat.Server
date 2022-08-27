using System;
namespace Sixpence.WeChat.Common.Model
{
    public class BaseWxResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}


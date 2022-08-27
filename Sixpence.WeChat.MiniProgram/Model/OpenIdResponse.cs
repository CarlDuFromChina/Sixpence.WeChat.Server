using System;
namespace Sixpence.WeChat.MiniProgram.Model
{

    /// <summary>
    /// <see cref="https://developers.weixin.qq.com/miniprogram/dev/OpenApiDoc/user-info/basic-info/getPluginOpenPId.html"/>
    /// </summary>
    public class OpenIdResponse
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string openpid { get; set; }
    }
}


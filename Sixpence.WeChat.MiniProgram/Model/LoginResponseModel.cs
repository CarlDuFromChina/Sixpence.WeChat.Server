using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sixpence.WeChat.Common.Model;

namespace Sixpence.WeChat.MiniProgram.Model
{
    [DataContract]
    public class LoginResponseModel : BaseWxResponse
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [DataMember]
        public string openid { get; set; }

        /// <summary>
        /// 会话密钥
        /// </summary>
        [DataMember]
        public string session_key { get; set; }

        /// <summary>
        /// 用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台帐号下会返回
        /// </summary>
        public string unionid { get; set; }

        /// <summary>
        /// token
        /// </summary>
        [JsonIgnore]
        [DataMember]
        public string token { get; set; }
    }
}


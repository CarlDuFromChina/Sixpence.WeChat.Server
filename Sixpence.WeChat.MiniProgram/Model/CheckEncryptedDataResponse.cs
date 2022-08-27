using System;
using Sixpence.WeChat.Common.Model;

namespace Sixpence.WeChat.MiniProgram.Model
{
    public class CheckEncryptedDataResponse : BaseWxResponse
    {
        public bool vaild { get; set; }
        public long create_time { get; set; }
    }
}


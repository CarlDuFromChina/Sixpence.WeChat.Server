using System;
using Sixpence.WeChat.Common.Model;

namespace Sixpence.WeChat.MiniProgram.Model
{
    public class PhoneNumberResponse : BaseWxResponse
    {
        public PhoneNumberInfo phone_info { get; set; }
    }

    public class PhoneNumberInfo
    {
        public string phoneNumber { get; set; }
        public string purePhoneNumber { get; set; }
        public string countryCode { get; set; }
        public WXWatermark watermark { get; set; }
    }
}


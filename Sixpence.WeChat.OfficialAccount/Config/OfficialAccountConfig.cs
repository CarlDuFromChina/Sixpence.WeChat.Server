﻿using Sixpence.Web.Config;
using Sixpence.Common.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Sixpence.WeChat.OfficialAccount
{
    public class OfficialAccountConfig : BaseSysConfig<OfficialAccountConfig>
    {
        public string Token { get; set; }

        public string Appid { get; set; }

        public string Secret { get; set; }

        public string EncodingAESKey { get; set; }
    }
}
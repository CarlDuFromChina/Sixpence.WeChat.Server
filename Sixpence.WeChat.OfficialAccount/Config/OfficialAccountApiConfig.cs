using Microsoft.Extensions.Configuration;
using Sixpence.Common.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.WeChat.OfficialAccount
{
    public class OfficialAccountApiConfig
    {
        public static IConfiguration Configuration { get; private set; }

        static OfficialAccountApiConfig()
        {
            Configuration = new JsonConfig("officialaccount.api.json").Configuration;
        }

        public static T GetValue<T>(string keyName)
        {
            return Configuration.GetValue<T>(keyName);
        }

        public static string GetValue(string keyName)
        {
            return GetValue<string>(keyName);
        }
    }
}

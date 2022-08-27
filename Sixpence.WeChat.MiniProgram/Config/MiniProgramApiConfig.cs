using System;
using Microsoft.Extensions.Configuration;
using Sixpence.Common.Config;

namespace Sixpence.WeChat.MiniProgram
{
    public class MiniProgramApiConfig
    {
        public static IConfiguration Configuration { get; private set; }

        static MiniProgramApiConfig()
        {
            Configuration = new JsonConfig("miniprogram.api.json").Configuration;
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


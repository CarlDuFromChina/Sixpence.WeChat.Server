using System;
using Sixpence.WeChat;

namespace Sixpence.WeChat.MiniProgram
{
    public class MiniProgramConfig : BaseSysConfig<MiniProgramConfig>
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}


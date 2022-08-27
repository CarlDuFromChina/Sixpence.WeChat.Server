using System;
using Sixpence.Common.Utils;
using Sixpence.WeChat.Common.Model;

namespace Sixpence.WeChat.Common.Util
{
    public class WxAssertUtil
    {
        public static void CheckWXResponse<T>(T t) where T: BaseWxResponse, new()
        {
            AssertUtil.IsTrue(t.errcode == 0, "微信接口请求失败：" + t.errmsg);
        }
    }
}


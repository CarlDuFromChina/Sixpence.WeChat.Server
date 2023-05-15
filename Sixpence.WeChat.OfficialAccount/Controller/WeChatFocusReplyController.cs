using System;
using Microsoft.AspNetCore.Mvc;
using Sixpence.Web.WebApi;
using Sixpence.WeChat.OfficialAccount.Service;

namespace Sixpence.WeChat.OfficialAccount.Controller
{
    public class WechatFocusReplyController : BaseApiController
    {
        [HttpGet]
        public string GetReplyMessage()
        {
            return new WeChatFocusReplyService().GetReplyMessage();
        }

        [HttpPut]
        public void UpdateReplyMessage(dynamic dto)
        {
            string text = dto?.text;
            new WeChatFocusReplyService().SaveReplyMessage(text);
        }
    }
}


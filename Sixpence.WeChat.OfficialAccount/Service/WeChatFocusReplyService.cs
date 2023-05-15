using log4net;
using Sixpence.Web.WebApi;
using Sixpence.ORM.Entity;
using System;
using System.Collections.Generic;
using Sixpence.Common.Logging;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Service;
using Sixpence.WeChat.OfficialAccount.Model.Message;

namespace Sixpence.WeChat.OfficialAccount.Service
{
    public class WeChatFocusReplyService
    {
        private const string FOCUS_PARAMETER_VALUE = "focus_reply";
        private IEntityManager Manager { get; set; }

        public WeChatFocusReplyService()
        {
            Manager = EntityManagerFactory.GetManager();
        }

        public void SaveReplyMessage(string text)
        {
            var sql = "update sys_config set value = @value where code = @code";
            var param = new
            {
                value = text,
                code = FOCUS_PARAMETER_VALUE
            };
            Manager.Execute(sql, param);
        }

        public string GetReplyMessage()
        {
            return new SysConfigService(Manager).GetValue(FOCUS_PARAMETER_VALUE)?.ToString();
        }

        /// <summary>
        /// 获取事件回复
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string GetReplyMessage(BaseWeChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.EventName) && message.EventName.Trim() == "subscribe")
            {
                var reply = GetReplyMessage();
                new FocusUserService().SaveData(message.FromUserName);
                return string.Format(WeChatMessageTemplate.Text, message.FromUserName, message.ToUserName, DateTime.Now.Ticks, reply);
            }
            return "";
        }
    }
}

﻿#region 类文件描述
/*********************************************************
Copyright @ Sixpence Studio All rights reserved. 
Author   : Karl Du
Created: 2020/11/1 20:30:09
Description：微信关键词回复 Service
********************************************************/
#endregion

using Sixpence.Web.WebApi;
using Sixpence.ORM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Service;
using Sixpence.WeChat.OfficialAccount.Entity;
using Sixpence.WeChat.OfficialAccount.Model.Message.Text;
using Sixpence.WeChat.OfficialAccount.Model.Message;

namespace Sixpence.WeChat.OfficialAccount.Service
{
    public class WeChatKeywordsService : EntityService<WechatKeywords>
    {
        #region 构造函数
        public WeChatKeywordsService() : base() { }

        public WeChatKeywordsService(IEntityManager manager) : base(manager) { }
        #endregion

        /// <summary>
        /// 根据关键词获取回复消息
        /// </summary>
        /// <param name="requestMesage"></param>
        /// <returns></returns>
        public IEnumerable<WechatKeywords> GetDataList(string requestMesage)
        {
            var sql = @"
SELECT * FROM wechat_keywords
WHERE name LIKE CONCAT('%', @name, '%')
";
            return Manager.Query<WechatKeywords>(sql, new Dictionary<string, object>() { { "@name", requestMesage } });
        }

        /// <summary>
        /// 关键词回复
        /// </summary>
        /// <param name="requestMsg"></param>
        /// <returns></returns>
        public string GetReplyMessage(WeChatTextMessage message)
        {
            var responseMsg = GetDataList(message.Content)?.FirstOrDefault()?.reply_content;
            if (string.IsNullOrEmpty(responseMsg))
            {
                responseMsg = "对不起，我听不懂你在说什么";
            }

            return string.Format(WeChatMessageTemplate.Text, message.FromUserName, message.ToUserName, DateTime.Now.Ticks, responseMsg);
        }
    }
}

﻿using Sixpence.Web.Config;
using Sixpence.Web.Store;
using Sixpence.Common;
using Sixpence.Common.IoC;
using Sixpence.Common.Utils;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixpence.Web.Service;
using Sixpence.Web.Entity;
using Sixpence.WeChat.OfficialAccount.Entity;
using Sixpence.WeChat.OfficialAccount.WeChatApi;

namespace Sixpence.WeChat.OfficialAccount.Service
{
    public class WeChatNewsMaterialService : EntityService<WechatNewsMaterial>
    {
        #region 构造函数
        public WeChatNewsMaterialService() : base() { }

        public WeChatNewsMaterialService(IEntityManager manager) : base(manager) { }
        #endregion

        /// <summary>
        /// 创建图文消息素材
        /// </summary>
        /// <param name="fileid"></param>
        /// <returns></returns>
        public string CreateData(string fileid)
        {
            var file = Manager.QueryFirst<SysFile>(fileid);
            var data = Manager.QueryFirst<WechatNewsMaterial>("SELECT * FROM wechat_news_material WHERE fileid = @id", new Dictionary<string, object>() { { "@id", fileid } });
            if (data == null)
            {
                var stream = ServiceContainer.Resolve<IStoreStrategy>(StoreConfig.Config.Type).GetStream(fileid);
                data = new WechatNewsMaterial()
                {
                    id = Guid.NewGuid().ToString(),
                    fileid = fileid
                };
                data.media_url = OfficialAccountApi.UploadImg(stream, file.name, file.content_type)?.url;
                data.local_url = SysFileService.GetDownloadUrl(fileid, false);
                CreateData(data);
            }
            return data.media_url;
        }

        /// <summary>
        /// 转换Url为微信媒体Url
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public string ConvertLocalUrlToWeChatUrl(string htmlContent)
        {
            var picList = HtmlUtil.GetHtmlImageUrlList(htmlContent).Distinct();
            if (!picList.IsEmpty())
            {
                var dic = new Dictionary<string, string>(); // 替换url
                picList.Each(item =>
                {
                    var start = item.IndexOf("objectId=") + 9;
                    var fileid = item.Substring(start, item.Length - start);
                    var url = CreateData(fileid);
                    dic.TryAdd(item, url);
                });
                htmlContent = htmlContent.Replace(dic);
            }
            return htmlContent;
        }

        /// <summary>
        /// 转换微信素材地址为本地地址
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public string ConvertWeChatUrlToLocalUrl(string htmlContent)
        {
            #region 更换data-src为src
            var imageTags = HtmlUtil.GetHtmlImagelList(htmlContent);
            if (!imageTags.IsEmpty())
            {
                var dic = new Dictionary<string, string>(); // 替换url
                imageTags.Each(item =>
                {
                    dic.TryAdd(item, item.Replace("data-src", "src"));
                });
                htmlContent = htmlContent.Replace(dic);
            }
            #endregion

            #region 更换地址为本地地址
            var picList = HtmlUtil.GetHtmlImageUrlList(htmlContent).Distinct();
            if (!picList.IsEmpty())
            {
                var dic = new Dictionary<string, string>(); // 替换url
                picList.Each(item =>
                {
                    var identity = item.Split("/")[4];
                    var sql = "SELECT * FROM wechat_news_material WHERE media_url like concat('%', @url, '%')";
                    var data = Manager.QueryFirst<WechatNewsMaterial>(sql, new Dictionary<string, object>() { { "@url", identity } });
                    if (data != null)
                    {
                        dic.TryAdd(item, data.local_url);
                    }
                });
                htmlContent = htmlContent.Replace(dic);
            }
            #endregion

            return htmlContent;
        }
    }
}

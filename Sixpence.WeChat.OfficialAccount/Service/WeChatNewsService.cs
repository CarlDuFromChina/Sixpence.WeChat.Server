﻿using Newtonsoft.Json;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Service;
using Sixpence.WeChat.OfficialAccount.Entity;
using Sixpence.WeChat.OfficialAccount.Model;
using Sixpence.WeChat.OfficialAccount.WeChatApi;
using System.Collections.Generic;

namespace Sixpence.WeChat.OfficialAccount.Service
{
    public class WeChatNewsService : EntityService<WechatNews>
    {
        #region 构造函数
        public WeChatNewsService() : base() { }

        public WeChatNewsService(IEntityManager manager) : base(manager) { }
        #endregion

        /// <summary>
        /// 删除图文素材
        /// </summary>
        /// <param name="ids"></param>
        public override void DeleteData(List<string> ids)
        {
            foreach (var item in ids)
            {
                var data = Manager.QueryFirst<WechatNews>(item);
                OfficialAccountApi.DeleteMaterial(data.media_id);
            }
            base.DeleteData(ids);
        }

        /// <summary>
        /// 修改图文素材
        /// </summary>
        /// <param name="t"></param>
        public override void UpdateData(WechatNews t)
        {
            var model = new
            {
                t.media_id,
                index = 0,
                articles = new
                {
                    title = t.name,
                    t.thumb_media_id,
                    t.author,
                    t.digest,
                    show_cover_pic = 1,
                    content = new WeChatNewsMaterialService(Manager).ConvertLocalUrlToWeChatUrl(t.html_content),
                    content_source_url = ""
                }
            };
            OfficialAccountApi.UpdateNews(JsonConvert.SerializeObject(model));
            base.UpdateData(t);
        }

        /// <summary>
        /// 获取图文素材
        /// </summary>
        /// <param name="pageIndex">从全部素材的该偏移位置开始返回，0表示从第一个素材 返回</param>
        /// <param name="pageSize">返回素材的数量，取值在1到20之间</param>
        /// <returns></returns>
        public WeChatNewsResponse GetDataList(int pageIndex, int pageSize)
        {
            var result = OfficialAccountApi.BatchGetMaterial("news", pageIndex, pageSize);
            var materialList = JsonConvert.DeserializeObject<WeChatNewsResponse>(result);
            return materialList;
        }

        /// <summary>
        /// 添加图文素材
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="thumb_media_id">图文消息的封面图片素材id（必须是永久mediaID）</param>
        /// <param name="author">作者</param>
        /// <param name="digest">图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空。如果本字段为没有填写，则默认抓取正文前64个字。</param>
        /// <param name="show_cover_pic">是否显示封面，0为false，即不显示，1为true，即显示</param>
        /// <param name="content">图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS,涉及图片url必须来源 "上传图文消息内的图片获取URL"接口获取。外部图片url将被过滤。</param>
        /// <param name="content_source_url">图文消息的原文地址，即点击“阅读原文”后的URL</param>
        /// <param name="need_open_comment">Uint32 是否打开评论，0不打开，1打开</param>
        /// <param name="only_fans_can_comment">Uint32 是否粉丝才可评论，0所有人可评论，1粉丝才可评论</param>
        public string CreateData(string title, string thumb_media_id, string author, string digest, bool show_cover_pic, string content, string content_source_url, bool need_open_comment, bool only_fans_can_comment)
        {
            var postData = new
            {
                articles = new List<object>()
                {
                    new
                    {
                        title,
                        thumb_media_id,
                        author,
                        digest,
                        show_cover_pic = show_cover_pic ? 1 : 0,
                        content = new WeChatNewsMaterialService(Manager).ConvertLocalUrlToWeChatUrl(content),
                        content_source_url,
                        need_open_comment = need_open_comment ? 1 : 0,
                        only_fans_can_comment = only_fans_can_comment ? 1: 0
                    }
                }
            };
            var result = OfficialAccountApi.AddNews(JsonConvert.SerializeObject(postData));
            var data = new WechatNews()
            {
                id = result.media_id,
                media_id = result.media_id,
                author = author,
                html_content = content,
                name = title,
                digest = digest,
                thumb_media_id = thumb_media_id
            };
            return CreateData(data);
        }
    }
}

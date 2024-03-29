﻿using Newtonsoft.Json;
using Sixpence.Common;
using Sixpence.Common.IoC;
using Sixpence.Common.Utils;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Config;
using Sixpence.Web.Model;
using Sixpence.Web.Service;
using Sixpence.Web.Store;
using Sixpence.WeChat.Common.Model;
using Sixpence.WeChat.OfficialAccount.Entity;
using Sixpence.WeChat.OfficialAccount.Extension;
using Sixpence.WeChat.OfficialAccount.Model;
using Sixpence.WeChat.OfficialAccount.WeChatApi;
using System;
using System.Collections.Generic;

namespace Sixpence.WeChat.OfficialAccount.Service
{
    public class WeChatMaterialService : EntityService<WechatMaterial>
    {
        #region 构造函数
        public WeChatMaterialService() : base() { }
        public WeChatMaterialService(IEntityManager manager) : base(manager) { }
        #endregion

        public override IList<EntityView> GetViewList()
        {
            var sql = $"SELECT * FROM wechat_material WHERE 1=1";
            return new List<EntityView>()
            {
                new EntityView()
                {
                    Sql = sql,
                    CustomFilter = new List<string>() { "name" },
                    OrderBy = "created_at, name",
                    ViewId = ""
                }
            };
        }

        /// <summary>
        /// 获取素材
        /// </summary>
        /// <param name="type">素材的类型，图片（image）、视频（video）、语音 （voice）、图文（news）</param>
        /// <param name="pageIndex">从全部素材的该偏移位置开始返回，0表示从第一个素材 返回</param>
        /// <param name="pageSize">返回素材的数量，取值在1到20之间</param>
        /// <returns></returns>
        public WeChatOtherMaterial GetMaterial(string type, int pageIndex, int pageSize)
        {
            var result = OfficialAccountApi.BatchGetMaterial(type, pageIndex, pageSize);
            var materialList = JsonConvert.DeserializeObject<WeChatOtherMaterial>(result);
            if (materialList == null || materialList.item == null || materialList.item.Count <= 0)
            {
                return materialList;
            }

            materialList.item.Each(item =>
            {
                item.UpdateTime = item.update_time.ToDateTimeString();
            });
            return materialList;
        }

        /// <summary>
        /// 上传素材（已上传则直接返回）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public WeChatSuccessUploadResponse CreateData(MaterialType type, string fileId)
        {
            var file = new SysFileService().GetData(fileId);
            AssertUtil.IsNull(file, $"根据fileid：{fileId}未找到记录");

            // 检查素材库是否已经上传
            var data = Manager.QueryFirst<WechatMaterial>("select * from wechat_material where sys_fileid = @sys_fileid", new Dictionary<string, object>() { { "@sys_fileid", file.id } });
            if (data != null)
            {
                return new WeChatSuccessUploadResponse()
                {
                    media_id = data.media_id,
                    url = data.url
                };
            }

            // 获取文件流
            var config = StoreConfig.Config;
            var stream = ServiceContainer.Resolve<IStoreStrategy>(config?.Type).GetStream(fileId);
            var media = OfficialAccountApi.AddMaterial(type, stream, file.name, file.content_type);
            stream.Dispose();

            // 创建素材记录
            var material = new WechatMaterial()
            {
                id = Guid.NewGuid().ToString(),
                media_id = media.media_id,
                url = media.url,
                sys_fileid = fileId,
                local_url = SysFileService.GetDownloadUrl(fileId),
                name = file.name,
                type = type.ToMaterialTypeString()
            };
            CreateData(material);
            return media;
        }
    }
}

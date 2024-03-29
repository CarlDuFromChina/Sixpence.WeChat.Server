﻿#region 类文件描述
/*********************************************************
Copyright @ Sixpence Studio All rights reserved. 
Author   : Karl Du
Created: 2020/11/18 22:05:00
Description：同步微信素材Job
********************************************************/
#endregion

using Quartz;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;
using Sixpence.Web.Job;
using Sixpence.WeChat.OfficialAccount.Entity;
using Sixpence.WeChat.OfficialAccount.Extension;
using Sixpence.WeChat.OfficialAccount.Model;
using Sixpence.WeChat.OfficialAccount.Service;
using System;

namespace Sixpence.WeChat.OfficialAccount.Job
{
    public class SyncMaterialJob : JobBase
    {
        public override string Name => "同步微信素材";

        public override string Description => "同步微信素材";

        public override IScheduleBuilder ScheduleBuilder => CronScheduleBuilder.CronSchedule("0 0 4 * * ?");

        public override TriggerState DefaultTriggerState => TriggerState.Paused;

        public override void Executing(IJobExecutionContext context)
        {
            var Manager = EntityManagerFactory.GetManager();
            Logger.Debug("开始同步微信公众号素材");
            var user = Manager.QueryFirst<UserInfo>("5B4A52AF-052E-48F0-82BB-108CC834E864");
            try
            {
                var images = new WeChatMaterialService(Manager).GetMaterial(MaterialType.image.ToMaterialTypeString(), 1, 5000);
                if (images.item_count == 0)
                {
                    Logger.Debug($"未发现图片待同步");
                }
                else
                {
                    Logger.Debug($"发现共{images.item_count}张图片待同步");
                    images.item.ForEach(item =>
                    {
                        var data = Manager.QueryFirst<WechatMaterial>(item.media_id);
                        if (data == null)
                        {
                            var material = new WechatMaterial()
                            {
                                id = item.media_id,
                                media_id = item.media_id,
                                url = item.url,
                                name = item.name,
                                type = MaterialType.image.ToMaterialTypeString(),
                                created_by = user.id,
                                created_by_name = user.name,
                                updated_by = user.id,
                                updated_by_name = user.name,
                                updated_at = DateTime.Now,
                                created_at = DateTime.Now
                            };
                            Manager.Create(material);
                        }
                        Logger.Debug($"同步图片{item.name}成功");
                    });
                    Logger.Debug($"微信图片素材同步成功，共同步{images.item_count}个");
                }


                var voices = new WeChatMaterialService(Manager).GetMaterial(MaterialType.voice.ToMaterialTypeString(), 1, 5000);
                if (voices.item_count == 0)
                {
                    Logger.Debug($"未发现语音待同步");
                }
                else
                {
                    Logger.Debug($"发现共{images.item_count}个语音待同步");
                    voices.item.ForEach(item =>
                    {
                        var data = Manager.QueryFirst<WechatMaterial>(item.media_id);
                        if (data == null)
                        {
                            var material = new WechatMaterial()
                            {
                                id = item.media_id,
                                media_id = item.media_id,
                                url = item.url,
                                sys_fileid = "",
                                name = item.name,
                                type = MaterialType.voice.ToMaterialTypeString(),
                                created_by = user.id,
                                created_by_name = user.name,
                                updated_by = user.id,
                                updated_by_name = user.name,
                                updated_at = DateTime.Now,
                                created_at = DateTime.Now
                            };
                            Manager.Create(material);
                        }
                        Logger.Debug($"同步语音{item.name}成功");
                    });
                    Logger.Debug($"微信语音素材同步成功，共同步{images.item_count}个");
                }


                var videos = new WeChatMaterialService(Manager).GetMaterial(MaterialType.video.ToMaterialTypeString(), 1, 5000);
                if (voices.item_count == 0)
                {
                    Logger.Debug($"未发现视频待同步");
                }
                else
                {
                    Logger.Debug($"发现共{images.item_count}个视频待同步");
                    videos.item.ForEach(item =>
                    {
                        var data = Manager.QueryFirst<WechatMaterial>(item.media_id);
                        if (data == null)
                        {
                            var material = new WechatMaterial()
                            {
                                id = item.media_id,
                                media_id = item.media_id,
                                url = item.url,
                                sys_fileid = "",
                                name = item.name,
                                type = MaterialType.video.ToMaterialTypeString(),
                                created_by = user.id,
                                created_by_name = user.name,
                                updated_by = user.id,
                                updated_by_name = user.name,
                                updated_at = DateTime.Now,
                                created_at = DateTime.Now
                            };
                            Manager.Create(material);
                            Logger.Debug($"同步视频{item.name}成功");
                        }
                    });
                    Logger.Debug($"微信视频素材同步成功，共同步{videos.item_count}个");
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"微信素材同步失败");
                Logger.Error($"{ex.Message}\r\n{ex.StackTrace}");
            }
        }
    }
}

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Sixpence.Common.Utils;
using Sixpence.Common;
using Sixpence.WeChat.Common.Model;
using Sixpence.WeChat.OfficialAccount.Model;
using Sixpence.WeChat.OfficialAccount.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.WeChat.OfficialAccount.Extension;

namespace Sixpence.WeChat.OfficialAccount.WeChatApi
{
    /// <summary>
    /// 公众号素材
    /// </summary>
    public partial class OfficialAccountApi
    {
        /// <summary>
        /// 批量获取微信素材
        /// </summary>
        public static string BatchGetMaterial(string type, int pageIndex, int pageSize)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("BatchGetMaterialApi"), OfficialAccountService.AccessToken);
            var postData = new
            {
                type,
                offset = (pageIndex - 1) * pageSize,
                count = pageSize
            };
            var result = HttpUtil.Post(url, JsonConvert.SerializeObject(postData));
            var resultJson = JObject.Parse(result);

            AssertUtil.IsTrue(resultJson.GetValue("errcode") != null && resultJson.GetValue("errcode").ToString() != "0", "获取微信素材失败");
            return result;
        }

        /// <summary>
        /// 获取微信素材
        /// </summary>
        public static string GetMaterial(string media_id)
        {
            var result = HttpUtil.Post(string.Format(OfficialAccountApiConfig.GetValue("GetMaterialApi"), OfficialAccountService.AccessToken), JsonConvert.SerializeObject(new
            {
                media_id
            }));
            var resultJson = JObject.Parse(result);

            AssertUtil.IsTrue(resultJson.GetValue("errcode") != null && resultJson.GetValue("errcode").ToString() != "0", "获取微信素材失败");
            return result;
        }

        /// <summary>
        /// 新增永久素材API（图文）
        /// </summary>
        /// <param name="postData">参考： https://developers.weixin.qq.com/doc/offiaccount/Asset_Management/Adding_Permanent_Assets.html</param>
        public static WeChatAddNewsResponse AddNews(string postData)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("AddNewsApi"), OfficialAccountService.AccessToken);
            var result = HttpUtil.Post(url, postData);
            var resultJson = JObject.Parse(result);
            if (resultJson.GetValue("errcode") != null && resultJson.GetValue("errcode").ToString() != "0")
            {
                var error = JsonConvert.DeserializeObject<WeChatErrorResponse>(result);
                throw new SpException("添加图文素材失败：" + error.errmsg);
            }
            else
            {
                return JsonConvert.DeserializeObject<WeChatAddNewsResponse>(result);
            }
        }

        /// <summary>
        /// 上传图文消息素材
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static WeChatSuccessUploadResponse UploadImg(Stream stream, string fileName, string contentType)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("UploadImage"), OfficialAccountService.AccessToken);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            var postData = new UploadFile
            {
                Name = "media",
                Filename = fileName,
                ContentType = contentType,
                Data = bytes
            };
            var result = HttpUtil.Post(url, postData);
            var resultJson = JObject.Parse(result);
            if (resultJson.GetValue("errcode") != null && resultJson.GetValue("errcode").ToString() != "0")
            {
                var error = JsonConvert.DeserializeObject<WeChatErrorResponse>(result);
                throw new SpException("添加素材失败：" + error.errmsg);
            }
            else
            {
                return JsonConvert.DeserializeObject<WeChatSuccessUploadResponse>(result);
            }
        }

        /// <summary>
        /// 新增永久素材（音乐、视频、图片）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static WeChatSuccessUploadResponse AddMaterial(MaterialType type, Stream stream, string fileName, string contentType = "application/octet-stream")
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("AddMaterialAPi"), OfficialAccountService.AccessToken, type.ToMaterialTypeString());
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            var postData = new UploadFile
            {
                Name = "media",
                Filename = fileName,
                ContentType = contentType,
                Data = bytes
            };
            var result = HttpUtil.Post(url, postData);
            var resultJson = JObject.Parse(result);
            if (resultJson.GetValue("errcode") != null && resultJson.GetValue("errcode").ToString() != "0")
            {
                var error = JsonConvert.DeserializeObject<WeChatErrorResponse>(result);
                throw new SpException("添加素材失败：" + error.errmsg);
            }
            else
            {
                return JsonConvert.DeserializeObject<WeChatSuccessUploadResponse>(result);
            }
        }

        /// <summary>
        /// 修改永久图文素材
        /// </summary>
        /// <param name="model"></param>
        public static void UpdateNews(string postData)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("UpdateNewsApi"), OfficialAccountService.AccessToken);
            var result = HttpUtil.Post(url, postData);
            var resultJson = JObject.Parse(result);
            if (resultJson.GetValue("errcode") != null && resultJson.GetValue("errcode").ToString() != "0")
            {
                var error = JsonConvert.DeserializeObject<WeChatErrorResponse>(result);
                throw new SpException("修改素材失败：" + error.errmsg);
            }
        }

        /// <summary>
        /// 删除素材
        /// </summary>
        /// <param name="mediaId"></param>
        public static void DeleteMaterial(string mediaId)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("DeleteMaterialApi"), OfficialAccountService.AccessToken);
            var postData = new
            {
                media_id = mediaId
            };
            var result = HttpUtil.Post(url, JsonConvert.SerializeObject(postData));
            var resultJson = JObject.Parse(result);
            if (resultJson.GetValue("errcode") != null && resultJson.GetValue("errcode").ToString() != "0")
            {
                var error = JsonConvert.DeserializeObject<WeChatErrorResponse>(result);
                throw new SpException("删除素材失败：" + error.errmsg);
            }
        }
    }
}

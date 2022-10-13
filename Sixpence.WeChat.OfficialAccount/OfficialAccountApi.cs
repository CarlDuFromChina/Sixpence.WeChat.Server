using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sixpence.Common;
using Sixpence.Common.Utils;
using Sixpence.WeChat.Common.Model;
using Sixpence.WeChat.OfficialAccount.Model;
using System.IO;

namespace Sixpence.WeChat.OfficialAccount
{
    public class OfficialAccountApi
    {
        public static readonly string BaseUrl = OfficialAccountApiConfig.GetValue("Url");

        /// <summary>
        /// 获取微信Token
        /// </summary>
        public static (string AccessToken, int Expire) GetAccessToken(string appid, string secret)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("GetAccessTokenApi"), appid, secret);
            var resp = HttpUtil.Get(url);
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "获取微信授权失败");
            return (respJson.GetValue("access_token").ToString(), Convert.ToInt32(respJson.GetValue("expires_in").ToString()));
        }

        public static void CheckWeChatErrorResponse(JObject respJson, string message)
        {
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", message);
        }

        #region 微信素材
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
                media_id = media_id
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
        #endregion

        #region 用户
        /// <summary>
        /// 获取关注用户列表
        /// <para>next_openid（当公众号关注者数量超过10000时，可通过填写next_openid的值，从而多次拉取列表的方式来满足需求。）</para>
        /// </summary>
        public static string GetFocusUserList(string nextOpenId = "")
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetFocusUserListApi"), OfficialAccountService.AccessToken, nextOpenId));
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "获取关注用户列表失败");
            return resp;
        }

        /// <summary>
        /// 获取关注用户基本信息
        /// </summary>
        public static string GetFocusUser(string openId, string lang = "zh_CN")
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetFocusUserApi"), OfficialAccountService.AccessToken, openId, lang));
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "获取关注用户基本信息失败");
            return resp;
        }

        /// <summary>
        /// 批量获取关注用户基本信息
        /// </summary>
        public static string BatchGetFocusUser(string postData)
        {
            var resp = HttpUtil.Post(string.Format(OfficialAccountApiConfig.GetValue("BatchGetFocusUserApi"), OfficialAccountService.AccessToken), postData);
            var respJson = JObject.Parse(resp);
            AssertUtil.IsTrue(respJson.GetValue("errcode") != null && respJson.GetValue("errcode").ToString() != "0", "批量获取关注用户基本信息失败");
            return resp;
        }
        #endregion

        #region 菜单
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        public static string CreateMenu(string postData)
        {
            var resp = HttpUtil.Post(string.Format(OfficialAccountApiConfig.GetValue("CreateMenuApi"), OfficialAccountService.AccessToken), postData);
            CheckWeChatErrorResponse(JObject.Parse(resp), "创建菜单失败");
            return resp;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public static string GetMenus()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetMenuApi"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "获取微信菜单失败");
            return resp;
        }

        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <param name="postData"></param>
        public static void DeleteMenu()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("DeleteMenuApi"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "删除微信菜单失败");
        }
        #endregion

        #region 模板
        public static WeChatIndustryResponse GetIndustry()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetIndustry"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "获取行业信息失败");
            return JsonConvert.DeserializeObject<WeChatIndustryResponse>(resp);
        }

        public static WeChatTemplateResponse GetTemplate()
        {
            var resp = HttpUtil.Get(string.Format(OfficialAccountApiConfig.GetValue("GetTemplate"), OfficialAccountService.AccessToken));
            CheckWeChatErrorResponse(JObject.Parse(resp), "获取模板失败");
            return JsonConvert.DeserializeObject<WeChatTemplateResponse>(resp);
        }

        public static void DeleteTemplate(string templateId)
        {
            var url = string.Format(OfficialAccountApiConfig.GetValue("DeletetTemplate"), OfficialAccountService.AccessToken);
            var postData = new
            {
                template_id = templateId
            };
            var resp = HttpUtil.Post(url, JsonConvert.SerializeObject(postData));
            CheckWeChatErrorResponse(JObject.Parse(resp), "删除模板失败");
        }

        public static void SendTemplateMessage(string openid, string templateid, string data, string url = "", string appid = "", string pagepath = "")
        {
            var _url = string.Format(OfficialAccountApiConfig.GetValue("SendTemplateMessage"), OfficialAccountService.AccessToken);
            var postData = new
            {
                touser = openid,
                template_id = templateid,
                url,
                miniprogram = new
                {
                    appid,
                    pagepath
                },
                client_msg_id = openid + Guid.NewGuid().ToString(),
                data
            };
            var resp = HttpUtil.Post(_url, JsonConvert.SerializeObject(postData));
            CheckWeChatErrorResponse(JObject.Parse(resp), "删除模板失败");
        }
        #endregion
    }
}


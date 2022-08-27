using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Sixpence.WeChat.Common.Model;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace Sixpence.WeChat.Common.Util
{
    /// <summary>
    /// 微信小程序用户加密数据的解密
    /// </summary>
    /// <typeparam name="T">解密后的实体类型（需继承自 WXBaseResult 类）</typeparam>
    public class WXBizDataCrypt<T> where T : WXBaseWatermarkResult
    {
        private string _appId;
        private string _sessionKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appId">小程序的appid</param>
        /// <param name="sessionKey">用户在小程序登录后获取的会话密钥</param>
        public WXBizDataCrypt(string appId, string sessionKey)
        {
            _appId = appId;
            _sessionKey = sessionKey;
        }

        /// <summary>
        /// 检验数据的真实性，获取解密后的明文并反序列化.
        /// </summary>
        /// <param name="encryptedData">加密的用户数据</param>
        /// <param name="iv">与用户数据一同返回的初始向量</param>
        /// <returns>解密并反序列化后的实体（解密失败时返回实体类型的默认值）</returns>
        public T DecryptData(string encryptedData, string iv)
        {
            // 解密数据
            var result = Decrypt(encryptedData, iv);
            if (string.IsNullOrEmpty(result))
            {
                return default(T);
            }

            // 反序列化解密结果
            var jsonResolver = new PropertyIncludeSerializerContractResolver();
            jsonResolver.IncludeProperty(typeof(WXWatermark), "Appid");
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;
            var dataObj = JsonConvert.DeserializeObject<T>(result, serializerSettings);

            // 验证AppID
            if (dataObj.Watermark?.Appid != _appId)
            {
                return default(T);
            }

            return dataObj;
        }

        /// <summary>
        /// 使用AES解密用户数据
        /// </summary>
        /// <param name="encryptedData">加密的用户数据</param>
        /// <param name="iv">与用户数据一同返回的初始向量</param>
        /// <returns>解密后的字符串</returns>
        private string Decrypt(string encryptedData, string iv)
        {
            // 验证参数及密钥
            if (string.IsNullOrEmpty(_sessionKey) || _sessionKey.Length != 24)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(iv) || iv.Length != 24)
            {
                return string.Empty;
            }

            AesManaged aes = new AesManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.IV = Convert.FromBase64String(iv);
            aes.Key = Convert.FromBase64String(_sessionKey);
            aes.Padding = PaddingMode.PKCS7;

            var cipher = Convert.FromBase64String(encryptedData);
            byte[] decryptText = aes.CreateDecryptor().TransformFinalBlock(cipher, 0, cipher.Length);

            return Encoding.UTF8.GetString(decryptText);
        }

    }

    /// <summary>
    /// 属性包含序列化分解器
    /// </summary>
    public class PropertyIncludeSerializerContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// 需要序列化的类型属性的字典
        /// </summary>
        private readonly Dictionary<Type, HashSet<string>> _includes;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PropertyIncludeSerializerContractResolver()
        {
            _includes = new Dictionary<Type, HashSet<string>>();
        }

        /// <summary>
        /// 添加需要序列化的类型属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jsonPropertyNames"></param>
        public void IncludeProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_includes.ContainsKey(type))
                _includes[type] = new HashSet<string>();

            foreach (var prop in jsonPropertyNames)
                _includes[type].Add(prop);
        }

        /// <summary>
        /// 重载 DefaultContractResolver.CreateProperty 方法
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsInclude(property.DeclaringType, property.PropertyName))
            {
                property.ShouldSerialize = i => true;
                property.Ignored = false;
            }

            return property;
        }

        /// <summary>
        /// 判断属性是否在序列化属性字典中
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jsonPropertyName"></param>
        /// <returns></returns>
        private bool IsInclude(Type type, string jsonPropertyName)
        {
            if (!_includes.ContainsKey(type))
                return false;

            return _includes[type].Contains(jsonPropertyName);
        }
    }
}


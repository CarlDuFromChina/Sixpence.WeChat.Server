using System;
using Newtonsoft.Json;
using Sixpence.Common.Utils;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Module.SysConfig;

namespace Sixpence.WeChat
{
    public class BaseSysConfig<T> where T : class
    {
        public static T Config { get; set; }
        static BaseSysConfig()
        {
            var em = EntityManagerFactory.GetManager();
            var sql = "SELECT * FROM sys_config WHERE code = @code";
            var param = new
            {
                code = EntityCommon.UpperChartToLowerUnderLine(typeof(T).Name.Replace("Config", ""))
            };
            var result = em.DbClient.QueryFirst<sys_config>(sql, param);
            AssertUtil.IsNull(result, $"未找到[{typeof(T).Name}]配置节点");
            Config = JsonConvert.DeserializeObject<T>(result.value);
        }
    }
}


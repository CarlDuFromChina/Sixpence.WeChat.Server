using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;
using Sixpence.Web.Model;
using Sixpence.Web.Service;
using System;
using System.Collections.Generic;

namespace Sixpence.WeChat.Holiday
{
    public class HolidayService : EntityService<Holiday>
    {
        #region 构造函数
        public HolidayService() : base() { }

        public HolidayService(IEntityManager manager) : base(manager) { }
        #endregion

        public List<SelectOption> GetRepeatTimeOptions()
        {
            var options = new List<SelectOption>()
            {
                new SelectOption("不重复", ""),
            };

            for (int i = 0; i <= 31; i++)
            {
                options.Add(new SelectOption("每" + (i == 0 ? "" : i.ToString()), i == 0 ? "every" : i.ToString()));
            }

            return options;
        }

        public List<SelectOption> GetRepeatUnitOptions()
        {
            return new List<SelectOption>()
            {
                new SelectOption("不重复", ""),
                new SelectOption("天重复", "day"),
                new SelectOption("月重复", "month"),
                new SelectOption("年重复", "year"),
            };
        }

        public static string GetCronString(string time, string unit)
        {
            if (string.IsNullOrEmpty(time) || string.IsNullOrEmpty(unit))
            {
                return "";
            }

            if (time == "every")
            {
                switch (unit)
                {
                    case "day":
                        return $"0 0 0 8 * ? ";
                    case "month":
                        return "";
                    case "year":
                        return "";
                    default:
                        return "";
                }
            }

            return "";
        }
    }
}

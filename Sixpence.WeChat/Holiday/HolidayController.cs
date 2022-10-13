using Microsoft.AspNetCore.Mvc;
using Sixpence.Web.Entity;
using Sixpence.Web.WebApi;
using System.Collections.Generic;

namespace Sixpence.WeChat.Holiday
{
    public class HolidayController : EntityBaseController<holiday, HolidayService>
    {
        [HttpGet("repeat_time/options")]
        public List<SelectOption> GetRepeatTimeOptions()
        {
            return new HolidayService().GetRepeatTimeOptions();
        }

        [HttpGet("repeat_unit/options")]
        public List<SelectOption> GetRepeatUnitOptions()
        {
            return new HolidayService().GetRepeatUnitOptions();
        }
    }
}

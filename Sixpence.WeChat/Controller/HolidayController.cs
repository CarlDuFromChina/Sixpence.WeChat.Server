﻿using Microsoft.AspNetCore.Mvc;
using Sixpence.Web.Entity;
using Sixpence.Web.Model;
using Sixpence.Web.WebApi;
using Sixpence.WeChat.Entity;
using Sixpence.WeChat.Service;
using System.Collections.Generic;

namespace Sixpence.WeChat.Controller
{
    public class HolidayController : EntityBaseController<Holiday, HolidayService>
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

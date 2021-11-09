﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rep33.Data;
using System;
using Serilog;

namespace Rep33.WEB.Controllers
{
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        public ReportController(ILogger<ReportController> logger)
        {
            _logger = logger;
        }

        [Route("api/report/build")] 
        public IActionResult BuildAdmin([FromQuery] DateTime dateRep, bool isSave = false, bool useSavedData = false, bool rep_getfile = false)
        {
            Log.Information("Построение отчета вручную");

            var reportManager = new ReportManager(Common.RepKind.Admin, isSave, useSavedData);
            if (!reportManager.CreateReport(dateRep))
                return NoContent();

            //reportManager.SaveFile(); 

            //return "built";
            // return $"dateRep={dateRep}; isSave={isSave}; useSavedData={useSavedData}";

            if (rep_getfile)
            {
                var exelbin = reportManager.GetExcelData();
                if (exelbin == null)
                    return NoContent();
                else
                    return File(exelbin, "application/vnd.openxmlformats");
            }
            return Ok();
        }

        [Route("api/report/buildsend")]
        public void BuildMan([FromQuery] DateTime dateRep)
        {
            var reportManager = new ReportManager(Common.RepKind.Manual);
            if (!reportManager.CreateReport(dateRep))
                return;
            reportManager.SendMail(); 
            //return $"dateRep={dateRep}";
        }

    }

}

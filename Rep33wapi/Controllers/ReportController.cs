using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rep33.Data;
using System;

namespace Rep33.WEB.Controllers
{
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        public ReportController(ILogger<ReportController> logger)
        {
            _logger = logger;
        }

        [Route("api/report/build")]
        public string BuildAdmin([FromQuery] DateTime dateRep, bool isSave = false, bool useSavedData = false)
        {
            var reportManager = new ReportManager(Common.RepKind.Admin, isSave, useSavedData);
            if (!reportManager.CreateReport(dateRep))
                return "";
            reportManager.SaveFile();

            //return "built";

            return $"dateRep={dateRep}; isSave={isSave}; useSavedData={useSavedData}";
        }

        [Route("api/report/buildsend")]
        public void BuildMan([FromQuery] DateTime dateRep)
        {
            var reportManager = new ReportManager(Common.RepKind.Manual);
            if (!reportManager.CreateReport(dateRep))
                return;
            reportManager.SendMail();
            //reportManager.CreateReport(System.DateTime.Today, "d:\\Work\\Temp\\r33.xls", false, true);
            //return "builtsent";
        }

    }

}

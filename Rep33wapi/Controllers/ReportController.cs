using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rep33.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rep33.WEB.Controllers
{
    [ApiController]
    public class ReportController : Controller
    {

        [Route("api/report/build")]
        public string BuildAdmin([FromQuery] DateTime dateRep, bool isSave = false, bool useSavedData = false)
        {
            var reportManager = new ReportManager(Common.RepKind.Manual, isSave, useSavedData);
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
            reportManager.SaveFile();
            //reportManager.CreateReport(System.DateTime.Today, "d:\\Work\\Temp\\r33.xls", false, true);
            //return "builtsent";
        }

    }

}

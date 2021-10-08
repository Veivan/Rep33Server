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
        public void Get()
        {
            var reportManager = new ReportManager();
            reportManager.CreateReport(System.DateTime.Today, "d:\\Work\\Temp\\r33.xls", false, true);
        }

        [Route("api/report/buildsend")]
        public string GetParam()
        {
            return "builtsent";
        }

    }

}

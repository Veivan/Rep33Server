using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Rep33.Data;

namespace Rep33.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParamsController : Controller
    {
        // GET: api/params
        [HttpGet]
        public string GetParams()
        {
            string[] parnames = { "lanch_date", "lanch_time", "save2bd", "use_bd_data", "use_timer" };
            JObject o = new JObject();

            foreach (var parn in parnames)
            {
                var val = AppSettings.GetAppSetting(parn);
                o.Add(new JProperty(parn, val));
            }
            return o.ToString();
        }

        // PUT: api/params/save2bd/true
        [HttpPut("{key}/{val}")]
        public void SetParam(string key, string val)
        {
            AppSettings.SetAppSetting(key, val);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Rep33.Data;
using System.Text.Json;

namespace Rep33.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParamsController : Controller
    {
        string[] parnames = { "use_timer", "launch_time", "save2bd" };

        // GET: api/params
        [HttpGet]
        public string GetParams()
        {
            JObject o = new JObject();

            foreach (var parn in parnames)
            {
                var val = AppSettings.GetAppSetting(parn);
                o.Add(new JProperty(parn, val));
            }
            return o.ToString();
        }

        // POST: api/params
        [HttpPost]
        public void SetParams([FromBody] JsonElement json)
        {
            foreach (var key in parnames)
            {
                JsonElement prop;

                if (json.TryGetProperty(key, out prop))
                {
                    var val = prop.GetString();
                    if (!string.IsNullOrEmpty(val))
                    {
                        AppSettings.SetAppSetting(key, val);
                    }
                }
            } 
        }

        // PUT: api/params/save2bd/true
        [HttpPut("{key}/{val}")]
        public void SetParam(string key, string val)
        {
            AppSettings.SetAppSetting(key, val);
        }

    }
}

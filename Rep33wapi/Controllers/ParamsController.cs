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
        string[] parnames = { "use_timer", "launch_time", "emails" };

        /// <summary>
        /// Чтение параметров.
        /// GET: api/params
        /// </summary>      
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

        /// <summary>
        /// Задание значений параметров.
        /// POST: api/params
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST api/params
        ///     {
        ///        "use_timer": true,
        ///        "launch_time": "04:00"
        ///     }
        ///     
        /// </remarks>
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

        /// <summary>
        /// Задание значения параметра.
        /// PUT: api/params/{key}/{val}
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT api/params/use_timer/true
        ///     
        /// </remarks>
        [HttpPut("{key}/{val}")]
        public void SetParam(string key, string val)
        {
            AppSettings.SetAppSetting(key, val);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterMasterAPI.WeatherModels;

namespace WaterMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        WeatherAPI wapi = null;
        // GET: api/Weather
        [HttpGet]
        public List[] Get()
        {
            wapi = new WeatherAPI(34, 139);
            WeatherModel wm = wapi.GetForecast();

            return wm.list;
        }

        // GET: api/Weather/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Weather
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Weather/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

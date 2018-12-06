using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterMasterAPI.WeatherModels;
using WaterMasterAPI.Controllers;
using SensorModels;

namespace WaterMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        // 3rd party API 
        WeatherAPI weatherAPI = null;

        // Controllers
        SensorController sensorController = new SensorController();
        SensorDataController sensorDataController = new SensorDataController();
        UserController userController = new UserController();

        // Control properties for watering
        private int threeHourIntervals = 8;
        private int rainRequirementInMM = 5;

        // GET: api/Weather
        [HttpGet("{id}", Name = "Get")]
        public bool StartWatering (string mac)
        {
            // make sensor obj
            Sensor sensor = sensorController.GetSensor(mac);
            // make sensorData obj
            SensorData sensorData = sensorDataController.GetSensorData(mac);

            if (sensorData.Humidity < sensor.LimitLow)
            {
                return true;
            }
            else if (sensorData.Humidity < sensor.LimitUp)
            {
                // Get User
                User user = userController.GetUserGeo(sensor.FK_UserId);

                // Generate Weathermodels
                weatherAPI = new WeatherAPI(user.Lat, user.Lon);
                WeatherModel weatherModel = weatherAPI.GetForecast();

                double incommingRain = 0;
                for (int i = 0; i < threeHourIntervals; i++)
                {
                    incommingRain += weatherModel.list[i].rain._3h;
                }

                if (incommingRain <= rainRequirementInMM)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

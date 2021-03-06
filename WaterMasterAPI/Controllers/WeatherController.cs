﻿using System;
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

        // GET: api/Weather/
        [HttpGet]
        [Route("{mac}")]
        public Watering CheckWatering(string mac)
        {
            // make sensor obj
            Sensor sensor = sensorController.GetSensor(mac);
            // make sensorData obj
            SensorData sensorData = sensorDataController.GetSensorData(mac);
            // Get User
            User user = userController.GetUserGeo(sensor.FK_UserId);

            Watering wat = new Watering();
            wat.Port = sensorController.GetPort(mac).Port;

            if (sensorData.Humidity < sensor.LimitLow)
            {
                userController.UpdateWaterCount(user.Id);
                wat.Water = 1;
                return wat;
                //return true;
            }
            else if (sensorData.Humidity < sensor.LimitUp)
            {
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
                    userController.UpdateWaterCount(user.Id);
                    wat.Water = 1;
                    return wat;
                }
            }

            wat.Water = 0;
            return wat;

        }
    }
}

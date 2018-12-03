using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterMasterAPI.Model;
using WaterMasterAPI.WeatherModels;

namespace WaterMasterAPI
{
    public class WeatherAPI
    {
        private double _lat;
        private double _lon;

        private string _reqString;

        public WeatherAPI(double lat, double lon)
        {
            _lat = lat;
            _lon = lon;

            _reqString = "http://openweathermap.org/data/2.5/forecast?lat=" + _lat + "&lon=" + _lon + "&appid=b6907d289e10d714a6e88b30761fae22";
        }

        public WeatherModel GetForecast()
        {
            Forecast foreCast = new Forecast();
            return foreCast.HandleRequest(_reqString);
        }

        

    }
}

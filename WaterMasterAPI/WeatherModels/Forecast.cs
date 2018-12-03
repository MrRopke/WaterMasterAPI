using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Newtonsoft.Json;
using WaterMasterAPI.WeatherModels;

namespace WaterMasterAPI.Model
{
    public class Forecast
    {
        private List<Rain> _rainList = new List<Rain>();

        public Forecast()
        {
        }
        public WeatherModel HandleRequest(string url)
        {
            WeatherModel wm = null;

            using (HttpClient client = new HttpClient())
            {
                string content = client.GetStringAsync(url).Result;
                wm = JsonConvert.DeserializeObject<WeatherModel>(content);
            }
            return wm;
        }

    }
    //    public List<Rain> HandleRequest(string url)
    //    {
    //        XmlDocument xml = new XmlDocument();
    //        using (HttpClient client = new HttpClient())
    //        {
    //            string content = client.GetStringAsync(url).Result;
    //            xml.LoadXml(content);
    //        }

    //        XmlNodeList nodes = xml.GetElementsByTagName("time");

    //        int iterations = 0;
    //        foreach (XmlNode node in nodes)
    //        {
    //            XmlNode precipitation = node.ChildNodes[1];

    //            if(iterations < _numberOfhours) {
    //                if (node.ChildNodes[1].Name == "precipitation")
    //                {
    //                    Rain rain = new Rain();
    //                    rain.RainValue = Convert.ToDouble(precipitation.Attributes["value"].Value);
    //                    _rainList.Add(rain);
    //                    iterations++;
    //                }
    //            }
    //        }
    //        return _rainList;
    //    }

    //}
}

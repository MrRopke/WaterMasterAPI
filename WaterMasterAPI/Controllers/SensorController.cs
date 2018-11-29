using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorModels;
using System.Data.SqlClient;

namespace WaterMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private string connString = "Server=tcp:cybonspace.database.windows.net,1433;Initial Catalog=MyDB;Persist Security Info=False;User ID=cybon;Password=Password1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        // POST: api/Sensor
        [HttpPost]
        public void PostSensorData([FromBody] SensorData sensorData)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "INSERT INTO SensorData VALUES(@Humidity, @Date, @FK_MacAddress)";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                cmd.Parameters.AddWithValue("@Humidity", sensorData.Humidity);
                cmd.Parameters.AddWithValue("@Date", sensorData.Date);
                cmd.Parameters.AddWithValue("@FK_MacAddress", sensorData.FK_MacAddress);

                cmd.ExecuteNonQuery();
            }
        }

        public SensorData GetSensorData(string MacAddress)
        {
            SensorData sd = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "SELECT TOP 1 * FROM SensorData WHERE SensorData.FK_MacAddress = '" + MacAddress + "' ORDER BY SensorData.Id DESC";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sd = new SensorData { FK_MacAddress = reader[3].ToString(), Id = Convert.ToInt32(reader[0]), Humidity = Convert.ToDouble(reader[1]), Date = Convert.ToDateTime(reader[2]) };                       
                    }
                }

                reader.Close();
            }

            return sd;
        }

        //Okay
        // GET: api/Sensor/5
        [HttpGet("{MacAddress}", Name = "Get")]
        public Sensor GetSensor(string MacAddress)
        {
            SensorData sd = GetSensorData(MacAddress);
            Sensor sensor = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "SELECT * FROM Sensors WHERE MacAddress = '" + MacAddress + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sensor = new Sensor { MacAddress = reader[0].ToString(), Name = reader[1].ToString(), LimitUp = Convert.ToDouble(reader[2]), LimitLow = Convert.ToDouble(reader[3]), FK_UserId = Convert.ToInt32(reader[4]), Data = sd };
                    }
                }

                reader.Close();
            }
            return sensor;
        }

        //// GET: api/Sensor/5
        //[HttpGet("{UserId}", Name = "Get")]
        //public List<string> GetSensorsMacAddress(int UserId)
        //{
        //    List<string> macAddress = new List<string>();

        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        conn.Open();

        //        string sqlString = "SELECT MacAddress FROM SENSORS WHERE sensors.FK_UserId = " + UserId;
        //        SqlCommand cmd = new SqlCommand(sqlString, conn);

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                macAddress.Add(reader.ToString());
        //            }
        //        }

        //        reader.Close();
        //    }
        //    return macAddress;
        //}




        //// GET: api/Sensor
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Sensor/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Sensor
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Sensor/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

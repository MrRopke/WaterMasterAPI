using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorModels;

namespace WaterMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private string connString = "Server=tcp:cybonspace.database.windows.net,1433;Initial Catalog=MyDB;Persist Security Info=False;User ID=cybon;Password=Password1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // POST: api/SensorData
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

        // GET api/SensorData
        [HttpGet]
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

        // PUT: api/SensorData/5
        
    }
}

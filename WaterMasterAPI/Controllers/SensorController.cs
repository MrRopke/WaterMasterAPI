using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorModels;
using System.Data.SqlClient;
using WaterMasterAPI.Controllers;

namespace WaterMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private string connString = "Server=tcp:cybonspace.database.windows.net,1433;Initial Catalog=MyDB;Persist Security Info=False;User ID=cybon;Password=Password1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
          
        
        public int GetPiId(int id)
        {
            int PiId = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string getPortNumberString = "SELECT PiId FROM Users WHERE ID = " + id;

                SqlCommand cmd = new SqlCommand(getPortNumberString, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PiId = Convert.ToInt32(reader[0]);
                        //PortNumber = 6000 + Convert.ToInt32(reader[0]) + 1;
                    }
                }                
            }
            return PiId;
        }

        public void IncrementID(int PiId, int UserId)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string IncrementPiIdString = "UPDATE Users SET PiId = " + (PiId + 1) + " WHERE ID = " + UserId;

                SqlCommand cmd = new SqlCommand(IncrementPiIdString, conn);

                cmd.ExecuteNonQuery();
            }
        }

        [HttpPost]
        public void PostSensor([FromBody] Sensor sensor)
        {
            int id = GetPiId(sensor.FK_UserId);
            IncrementID(id, sensor.FK_UserId);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "INSERT INTO Sensors VALUES (@MacAddress, @Name, @LimitUp, @LimitLow, @FK_UserId, @Port)";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                cmd.Parameters.AddWithValue("@MacAddress", sensor.MacAddress);
                cmd.Parameters.AddWithValue("@Name", sensor.Name);
                cmd.Parameters.AddWithValue("@LimitUp", sensor.LimitUp);
                cmd.Parameters.AddWithValue("@LimitLow", sensor.LimitLow);
                cmd.Parameters.AddWithValue("@FK_UserId", sensor.FK_UserId);
                cmd.Parameters.AddWithValue("@Port", 6000 + (id + 1));

                cmd.ExecuteNonQuery();
            }
        }



        [HttpPut]
        public void UpdateSensor([FromBody] Sensor sensor)
        {
            using(SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "UPDATE Sensors SET Name = @Name, LimitUp = @LimitUp, LimitLow = @LimitLow WHERE MacAddress = '" + sensor.MacAddress + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                cmd.Parameters.AddWithValue("@Name", sensor.Name);
                cmd.Parameters.AddWithValue("@LimitUp", sensor.LimitUp);
                cmd.Parameters.AddWithValue("@LimitLow", sensor.LimitLow);

                cmd.ExecuteNonQuery();
            }
        }

        // GET: api/Sensor/5
        [HttpGet]
        [Route("Port/{MacAddress}")]
        public PiPortModel GetPort(string macAddress)
        {
            PiPortModel ppm = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "SELECT Port FROM Sensors WHERE MacAddress = '" + macAddress + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ppm = new PiPortModel { MacAddress = macAddress, Port = Convert.ToInt32(reader[0]) };
                    }
                }

                reader.Close();
            }
            return ppm;
        }

        // GET: api/Sensor/5
        [HttpGet]
        [Route("Mac/{MacAddress}")]
        public Sensor GetSensor(string MacAddress)
        {
            SensorDataController sdc = new SensorDataController();
            SensorData sd = sdc.GetSensorData(MacAddress);
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

        //GET: api/Sensor/5
        [HttpGet("{UserId}", Name = "GetMacs")]
        [Route("UserId/{UserId}")]
        public List<string> GetSensorsMacAddress(int UserId)
        {
            List<string> macAddress = new List<string>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "SELECT MacAddress FROM SENSORS WHERE sensors.FK_UserId = " + UserId;
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        macAddress.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return macAddress;
        }
    }
}

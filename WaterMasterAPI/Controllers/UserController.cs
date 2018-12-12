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
    public class UserController : ControllerBase
    {
        private string connString = "Server=tcp:cybonspace.database.windows.net,1433;Initial Catalog=MyDB;Persist Security Info=False;User ID=cybon;Password=Password1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        // GET: api/User/5
        [HttpGet()]
        [Route("login/{username}&&{password}")]
        public int Login(string username, string password)
        {
            int userId = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "SELECT id FROM Users WHERE Username = '" + username + "' AND Password = '" + password + "'"; ;
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userId = Convert.ToInt32(reader[0]);
                    }
                }
                reader.Close();
            }
            return userId;
        }

        [HttpGet()]
        [Route("usergeo/{id}")]
        public User GetUserGeo(int id)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "SELECT * FROM Users WHERE Id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user = new User { Id = id, Username = "", Password = "", Lat = Convert.ToDouble(reader[3]), Lon = Convert.ToDouble(reader[4]), WaterCount = Convert.ToInt16(reader[6]), LastWater = Convert.ToDateTime(reader[7]) };
                    }
                }
                reader.Close();
            }
            return user;
        }

        [HttpPut]
        public void UpdateWaterCount(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlString = "UPDATE Users SET WaterCount = WaterCount + 1, LastWater = GETDATE() WHERE Id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
            }
        }
    }
}

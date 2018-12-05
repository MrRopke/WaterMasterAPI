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
        [HttpGet("{id}", Name = "Get")]
        public string Login(int id)
        {
            return "value";
        }

        [HttpGet("{id}", Name = "Get")]
        public User GetUser(int id)
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
                        user = new User { Id = Convert.ToInt16(reader[0]), Username = reader[1].ToString(), Password = reader[2].ToString(), Lat = Convert.ToDouble(reader[3]), Lon = Convert.ToDouble(reader[4]) };
                    }
                }
                reader.Close();
            }
            return user;
        }
    }
}

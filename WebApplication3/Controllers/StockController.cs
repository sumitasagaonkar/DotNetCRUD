using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAblication.Repository;

namespace WebAblication.Controllers
{
   
    // route of the controller 
    
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StockController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("api/getvalues")]
        public JObject getvalue()
        {
            JObject result = new JObject();
            string body;
            try
            { 
                using (var reader = new StreamReader(Request.Body))
                {
                     body = reader.ReadToEnd();


                }
            }
            catch(Exception r)
            {
                throw r;
            }

            UserModel user = JsonConvert.DeserializeObject<UserModel>(body);
            string retval = "Sumit";
            var stockRepository = new StockRepository(_configuration);
            result = stockRepository.GetAllStocks() as JObject;



            return result;
        }

        [HttpPost]
        [Route("api/insertvalue")]
        public JObject insertValue()
        {
            JObject result = new JObject();
            string body;
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    body = reader.ReadToEnd();


                }
            
            UserModel user = JsonConvert.DeserializeObject<UserModel>(body);
           string _connString = _configuration.GetConnectionString("CommanderConnections");
            SqlConnection con = new SqlConnection(_connString);
            SqlCommand com = new SqlCommand("insert into stock values ( @ProductName ,@Quantity )", con);
                com.Parameters.AddWithValue("@ProductName",user.name);
                com.Parameters.AddWithValue("@Quantity",user.value);
                con.Open();
            int recordsAffected = com.ExecuteNonQuery();

            con.Close();
            if (recordsAffected>0)
            {
                result["status"] = "success";
                result["recordAffected"] = recordsAffected;
                
            }
            else
            {
                result["status"] = "faild";
                result["recordAffected"] = recordsAffected;

            }
            }
            catch (Exception r)
            {
                throw r;
            }



            return result;
        }


        [HttpPut]
        [Route("api/updateValue")]
        public JObject UpdateValue(string id)
        {
            JObject retval = new JObject();
           
            string body;
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    body = reader.ReadToEnd();
                }
                 UserModel user = JsonConvert.DeserializeObject<UserModel>(body);

                string entityId = id;
                string _connString = _configuration.GetConnectionString("CommanderConnections");
                SqlConnection con = new SqlConnection(_connString);
                SqlCommand com = new SqlCommand("update stock set ProductName = @ProductName, Quantity = @Quantity where id = @id ", con);
                com.Parameters.AddWithValue("@ProductName", user.name);
                com.Parameters.AddWithValue("@Quantity", user.value);
                com.Parameters.AddWithValue("@id", entityId);
                con.Open();
                int recordsAffected = com.ExecuteNonQuery();

                con.Close();
                if (recordsAffected > 0)
                {
                    retval["status"] = "success";
                    retval["recordAffected"] = recordsAffected;

                }
                else
                {
                    retval["status"] = "faild";
                    retval["recordAffected"] = recordsAffected;

                }

            }
            catch(Exception ex)
            {
                throw ex;
            }


             return retval;
        }

        [HttpPost]
        [Route("api/delete")]
        public JObject deleteValue(string id)
        {
            JObject retval = new JObject();
            string deletvaleId = id;
            try
            {
                string _connectionString = _configuration.GetConnectionString("CommanderConnections");
                SqlConnection con = new SqlConnection(_connectionString);
                SqlCommand com = new SqlCommand("delete from stock where id = @id", con);
                com.Parameters.AddWithValue("@id", id);
                con.Open();
                int recordsAffected = com.ExecuteNonQuery();
                con.Close();
                if(recordsAffected > 0)
                {
                    retval["status"] = "success";
                    retval["recordAffected"] = recordsAffected;
                }
                else
                {
                    retval["status"] = "faild";
                    retval["recordAffected"] = recordsAffected;
                }

            }
            catch(Exception ex)
            {
                throw ex;
               
            }



            return retval;
        }
        public class UserModel
        {
            public string name { get; set; }
            public string value { get; set; }
        }

    }
}

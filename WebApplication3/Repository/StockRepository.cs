using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAblication.Data;
using WebAblication.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAblication.Repository
{
    public class StockRepository
    {
        

        private string _connString;

        public StockRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("CommanderConnections");
        }

        public List<Stock> insetToStocks()
        {
            
            var stocks = new List<Stock>();
            SqlConnection con = new SqlConnection(_connString);
            SqlCommand com = new SqlCommand("insert into stock values('robot',15)", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();


         

            return stocks;
        }
        public Object GetAllStocks()
        {
            JObject jjsonResult = new JObject();
            JObject rtObject = null;

            var stocks = new List<Stock>();
            SqlConnection con = new SqlConnection(_connString);
            SqlCommand com = new SqlCommand("select ID, ProductName, Quantity from stock ", con);
            con.Open();
            SqlDataReader read = com.ExecuteReader();

            var r = Serialize(read);
            System.Text.StringBuilder stagging = new System.Text.StringBuilder();
             string jsonString = JsonConvert.SerializeObject(r, Formatting.Indented);
            string rootResultArryAttach = "{\"result\":" + jsonString + "}";
            rtObject = JObject.Parse(rootResultArryAttach);

            var stocksq = new List<Stock>();
            var stockss = new List<Stock>();
            con.Close();




            return rtObject;
        }
        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
    }
}
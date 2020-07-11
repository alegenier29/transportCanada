using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TransportCanada.API3.Models;
using System;

namespace TransportCanada.API3.Controllers
{

    [Route("tc/[controller]")]
    [ApiController]
    public class API3Controller : ControllerBase
    {

        // GET: /tc/API3
        // GET: /tc/API3?systemType=Brakes
        [HttpGet]
        public async Task<ActionResult<Recall>> GetRecallsFromSystemTypeAsync(string systemType)
        {

            string fileName = "data.json";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\json", fileName);
            string json = "";
            if (System.IO.File.Exists(filePath))
            {
                json = System.IO.File.ReadAllText(filePath);

            }
            List<Recall> recalls =  await Task.Run(() => JsonConvert.DeserializeObject<List<Recall>>(json));
            
            systemType ??= "";
            systemType.Trim();

            recalls = recalls
                .Where(x => x.SYSTEM_TYPE_ETXT.Contains(systemType, StringComparison.OrdinalIgnoreCase) || x.SYSTEM_TYPE_FTXT.Contains(systemType, StringComparison.OrdinalIgnoreCase))?
                .ToList();

            return Ok(recalls);
           
        }

         
        // POST: tc/API3
        [HttpPost]
        
        public async Task<ActionResult<Recall>> PostRecalls(List<Recall> recalls)
        {

            if (recalls == null || !recalls.Any())
            {
                return BadRequest("No body Content");
            }


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "API3");

            string api = "http://data.tc.gc.ca/v1.3/api/eng/vehicle-recall-database";
            string action = "recall-summary";
            string variableName = "recall-number";
            string query = "format=json";


            foreach (var recall in recalls)
            {

                string recallNumber = recall.recallNumber;

                if (!string.IsNullOrWhiteSpace(recallNumber)) 
                { 
                    string path = string.Format("{0}/{1}/{2}/{3}?{4}", api, action,variableName,recallNumber, query);
                    HttpResponseMessage responseJson = await client.GetAsync(path);
                    if (responseJson.IsSuccessStatusCode)
                    {
                        string recallSummary = await responseJson.Content.ReadAsStringAsync();
                        var response = await Task.Run(() => JsonConvert.DeserializeObject<TransportCanada.Models.Response>(recallSummary));
                        recall.SYSTEM_TYPE_FTXT = response.ResultSet.First().Where(x => x.Name.Equals("SYSTEM_TYPE_FTXT")).FirstOrDefault()?.Value.Literal;
                        recall.SYSTEM_TYPE_ETXT = response.ResultSet.First().Where(x => x.Name.Equals("SYSTEM_TYPE_ETXT")).FirstOrDefault()?.Value.Literal;

                    }
                }

            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(recalls, Formatting.Indented,settings);
            string fileName = "data.json";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\json", fileName);
            using (StreamWriter writer = System.IO.File.CreateText(filePath))
            {
                writer.WriteLine(json);
            }

          
           
            return CreatedAtAction("PostRecalls", recalls);
        }

    }
}

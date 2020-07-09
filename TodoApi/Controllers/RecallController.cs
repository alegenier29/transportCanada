using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TransportCanada.API3.Models;

namespace TransportCanada.API3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RecallController : ControllerBase
    {
        private readonly RecallContext _context;

        public RecallController(RecallContext context)
        {
            _context = context;
        }

        // GET: api/Recall
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recall>>> GetRecalls()
        {        
            
            return await _context.Recalls.ToListAsync();
        }

        // GET: api/Recall/1
        [HttpGet("{recallNumber}")]
        public async Task<ActionResult<Recall>> GetRecall(string recallNumber)
        {

            string fileName = "data.json";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\json", fileName);
            string json = System.IO.File.ReadAllText(filePath);

            List<Recall> recallsFull = JsonConvert.DeserializeObject<List<Recall>>(json);
            List<Recall> recalls = recallsFull.Where(x => x.recallNumber.Equals(recallNumber)).ToList();


           // _context.Recalls.AddRange(recallsFull);
           await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecall", recalls);
        }

        // PUT: api/Recall/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{recallNumber}")]
        public async Task<IActionResult> PutRecall(string recallNumber, Recall recall)
        {
            if (recallNumber != recall.recallNumber)
            {
                return BadRequest();
            }

            _context.Entry(recall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecallExists(recallNumber))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

  

        // POST: api/Recalls
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                        var response = JsonConvert.DeserializeObject<TransportCanada.Models.Response>(recallSummary);
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


            //_context.Recalls.AddRange(recalls);
            //await _context.SaveChangesAsync();

            return Ok();
            //return CreatedAtAction("PostRecalls", recalls);
        }


    

        


        // DELETE: api/Recall/5
        [HttpDelete("{recallNumber}")]
        public async Task<ActionResult<Recall>> DeleteRecall(string recallNumber)
        {
            var recall = await _context.Recalls.FindAsync(recallNumber);
            if (recall == null)
            {
                return NotFound();
            }

            

            _context.Recalls.Remove(recall);
            await _context.SaveChangesAsync();

            return recall;
        }

        private bool RecallExists(string recallNumber)
        {
            return _context.Recalls.Any(e => e.recallNumber == recallNumber);
        }
    }
}

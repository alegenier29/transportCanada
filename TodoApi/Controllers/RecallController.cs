using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: api/Recall/5
        [HttpGet("{recallNumber}")]
        public async Task<ActionResult<Recall>> GetRecall(string recallNumber)
        {
            var recall = await _context.Recalls.FindAsync(recallNumber);

            if (recall == null)
            {
                return NotFound();
            }

            return recall;
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

        // POST: api/Recall
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Recall>> PostRecall(Recall recall)
        {
            _context.Recalls.Add(recall);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecall", new { recallNumber = recall.recallNumber }, recall);
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

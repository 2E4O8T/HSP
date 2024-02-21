using BookingApi.Data;
using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleBookingController : ControllerBase
    {
        private readonly BookingApiDbContext _context;

        public SimpleBookingController(BookingApiDbContext context)
        {
            _context = context;
        }

        // GET: api/SimpleBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SimpleBooking>>> GetAllSimpleBookings()
        {
          if (_context.SimpleBooking == null)
          {
              return NotFound();
          }
            return await _context.SimpleBooking.ToListAsync();
        }

        // GET: api/SimpleBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleBooking>> GetSimpleBookingById(int id)
        {
          if (_context.SimpleBooking == null)
          {
              return NotFound();
          }
            var simpleBooking = await _context.SimpleBooking.FindAsync(id);

            if (simpleBooking == null)
            {
                return NotFound();
            }

            return simpleBooking;
        }

        // PUT: api/SimpleBookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSimpleBookingById(int id, SimpleBooking simpleBooking)
        {
            if (id != simpleBooking.Id)
            {
                return BadRequest();
            }

            _context.Entry(simpleBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SimpleBookingExists(id))
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

        // POST: api/SimpleBookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SimpleBooking>> CreateSimpleBooking(SimpleBooking simpleBooking)
        {
          if (_context.SimpleBooking == null)
          {
              return Problem("Entity set 'BookingApiDbContext.SimpleBooking'  is null.");
          }
            _context.SimpleBooking.Add(simpleBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllSimpleBookings", new { id = simpleBooking.Id }, simpleBooking);
        }

        // DELETE: api/SimpleBookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSimpleBookingById(int id)
        {
            if (_context.SimpleBooking == null)
            {
                return NotFound();
            }
            var simpleBooking = await _context.SimpleBooking.FindAsync(id);
            if (simpleBooking == null)
            {
                return NotFound();
            }

            _context.SimpleBooking.Remove(simpleBooking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SimpleBookingExists(int id)
        {
            return (_context.SimpleBooking?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

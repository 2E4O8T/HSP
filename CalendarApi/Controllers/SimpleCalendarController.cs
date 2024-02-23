using CalendarApi.Data;
using CalendarApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CalendarApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleCalendarController : ControllerBase
    {
        private readonly CalendarApiDbContext _context;

        public SimpleCalendarController(CalendarApiDbContext context)
        {
            _context = context;
        }

        // GET: api/SimpleCalendar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SimpleCalendar>>> GetAllSimpleCalendars()
        {
          if (_context.SimpleCalendar == null)
          {
              return NotFound();
          }
            return await _context.SimpleCalendar.ToListAsync();
        }

        // GET: api/SimpleCalendar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleCalendar>> GetSimpleCalendarById(int id)
        {
          if (_context.SimpleCalendar == null)
          {
              return NotFound();
          }
            var simpleCalendar = await _context.SimpleCalendar.FindAsync(id);

            if (simpleCalendar == null)
            {
                return NotFound();
            }

            return simpleCalendar;
        }

        // PUT: api/SimpleCalendar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSimpleCalendarById(int id, SimpleCalendar simpleCalendar)
        {
            if (id != simpleCalendar.Id)
            {
                return BadRequest();
            }

            _context.Entry(simpleCalendar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SimpleCalendarExists(id))
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

        // POST: api/SimpleCalendar
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SimpleCalendar>> CreateSimpleCalendar(SimpleCalendar simpleCalendar)
        {
          if (_context.SimpleCalendar == null)
          {
              return Problem("Entity set 'CalendarApiDbContext.SimpleCalendar'  is null.");
          }
            _context.SimpleCalendar.Add(simpleCalendar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllSimpleCalendars", new { id = simpleCalendar.Id }, simpleCalendar);
        }

        // DELETE: api/SimpleCalendar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSimpleCalendarById(int id)
        {
            if (_context.SimpleCalendar == null)
            {
                return NotFound();
            }
            var simpleCalendar = await _context.SimpleCalendar.FindAsync(id);
            if (simpleCalendar == null)
            {
                return NotFound();
            }

            _context.SimpleCalendar.Remove(simpleCalendar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SimpleCalendarExists(int id)
        {
            return (_context.SimpleCalendar?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Add check appointment availability
        [HttpGet("CheckAppointmentAvailability")]
        public async Task<ActionResult<bool>> CheckAppointmentAvailability(string consultantName, DateTime appointmentDate)    //string consultantName, DateTime appointmentDate
        {
            //var isAvailable = await _context.SimpleCalendar.AnyAsync(a => a.FormatedAppointmentDate == appointmentDate);
            var isAvailable = !await _context.SimpleCalendar.AnyAsync(a => a.ConsultantName == consultantName && a.AppointmentDate == appointmentDate);

            return isAvailable;
        }
    }
}

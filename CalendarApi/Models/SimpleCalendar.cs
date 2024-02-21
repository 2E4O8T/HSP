using System.ComponentModel.DataAnnotations;

namespace CalendarApi.Models
{
    public class SimpleCalendar
    {
        public int Id { get; set; }
        public string ConsultantName { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
    }
}

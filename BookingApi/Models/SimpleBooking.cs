using System.ComponentModel.DataAnnotations;

namespace BookingApi.Models
{
    public class SimpleBooking
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string ConsultantName { get; set; } = string.Empty;
        public string ConsultantSpecialty { get; set; } = string.Empty;   
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        public string FormatedAppointmentDate => AppointmentDate.ToString("dd/MM/yyyy");
    }
}

using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Ward
    {
        public int WardId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string WardType { get; set; } = string.Empty; // General, ICU, Maternity, Pediatric

        [Range(1, 200)]
        public int TotalBeds { get; set; }

        public string? Description { get; set; }

        public ICollection<Bed> Beds { get; set; } = new List<Bed>();
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    }
}

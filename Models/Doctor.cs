using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, Phone, StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        public int WardId { get; set; }
        [Display(Name = "Assigned Ward")]
        public Ward? Ward { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Consultation Schedule")]
        [StringLength(200)]
        public string ConsultationSchedule { get; set; } = string.Empty;

        [Display(Name = "Years of Experience")]
        [Range(0, 60)]
        public int YearsExperience { get; set; }

        [Display(Name = "On Duty")]
        public bool OnDuty { get; set; } = false;

        [Display(Name = "Joined Date")]
        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; } = DateTime.Now;

        public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
        public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
    }
}

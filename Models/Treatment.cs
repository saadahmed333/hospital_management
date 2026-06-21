using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Treatment
    {
        public int TreatmentId { get; set; }

        public int AdmissionId { get; set; }
        public Admission? Admission { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Treatment Name")]
        public string TreatmentName { get; set; } = string.Empty;

        [Required, StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required, StringLength(50)]
        [Display(Name = "Treatment Type")]
        public string TreatmentType { get; set; } = string.Empty; // Medication, Surgery, Therapy, Observation

        [StringLength(500)]
        public string? Medication { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Treatment Date")]
        public DateTime TreatmentDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Notes { get; set; }

        [Display(Name = "Cost")]
        [Range(0, 999999)]
        public decimal Cost { get; set; }
    }
}

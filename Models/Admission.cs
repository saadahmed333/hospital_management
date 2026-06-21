using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Admission
    {
        public int AdmissionId { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public int WardId { get; set; }
        public Ward? Ward { get; set; }

        public int BedId { get; set; }
        public Bed? Bed { get; set; }

        [Required]
        [Display(Name = "Admission Date")]
        [DataType(DataType.DateTime)]
        public DateTime AdmissionDate { get; set; } = DateTime.Now;

        [Display(Name = "Expected Discharge Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedDischargeDate { get; set; }

        [Display(Name = "Actual Discharge Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ActualDischargeDate { get; set; }

        [Required, StringLength(20)]
        [Display(Name = "Admission Type")]
        public string AdmissionType { get; set; } = "OPD"; // Emergency, OPD

        [Required, StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Discharged, Transferred

        [StringLength(500)]
        [Display(Name = "Chief Complaint")]
        public string? ChiefComplaint { get; set; }

        [StringLength(200)]
        [Display(Name = "Diagnosis")]
        public string? Diagnosis { get; set; }

        [StringLength(500)]
        [Display(Name = "Discharge Notes")]
        public string? DischargeNotes { get; set; }

        [Display(Name = "Stay Duration (Days)")]
        public int? StayDuration =>
            ActualDischargeDate.HasValue
            ? (int)(ActualDischargeDate.Value - AdmissionDate).TotalDays
            : (int)(DateTime.Now - AdmissionDate).TotalDays;

        public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
    }
}

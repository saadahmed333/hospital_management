using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Bed
    {
        public int BedId { get; set; }

        [Required, StringLength(20)]
        [Display(Name = "Bed Number")]
        public string BedNumber { get; set; } = string.Empty;

        public int WardId { get; set; }
        public Ward? Ward { get; set; }

        [Display(Name = "Is Occupied")]
        public bool IsOccupied { get; set; } = false;

        [Display(Name = "Bed Type")]
        [StringLength(50)]
        public string BedType { get; set; } = "Standard"; // Standard, ICU, Incubator, etc.

        public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    }
}

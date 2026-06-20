namespace HospitalManagement.ViewModels
{
    public class DashboardViewModel
    {
        // GCN = 2: Average patient stay duration + bed utilization trends
        public int GroupConfigNumber { get; set; } = 2;

        // Summary Stats
        public int TotalPatients { get; set; }
        public int ActiveAdmissions { get; set; }
        public int TotalDoctors { get; set; }
        public int DoctorsOnDuty { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int AvailableBeds { get; set; }
        public int PendingDischarges { get; set; }
        public int EmergencyAdmissionsToday { get; set; }
        public int AdmissionsLast24Hrs { get; set; }
        public double AverageStayDuration { get; set; }
        public int DischargedThisMonth { get; set; }
        public double BedUtilizationRate => TotalBeds > 0 ? Math.Round((double)OccupiedBeds / TotalBeds * 100, 1) : 0;

        // GCN=2 specific: average stay per ward
        public List<WardAvgStay> WardAvgStays { get; set; } = new();

        // Ward Occupancy
        public List<WardOccupancy> WardOccupancies { get; set; } = new();

        // Recent Admissions
        public List<RecentAdmission> RecentAdmissions { get; set; } = new();

        // Doctor Workload
        public List<DoctorWorkload> DoctorWorkloads { get; set; } = new();

        // Monthly Inflow (last 6 months)
        public List<MonthlyInflow> MonthlyInflows { get; set; } = new();
    }

    public class WardAvgStay
    {
        public string WardName { get; set; } = string.Empty;
        public double AvgDays { get; set; }
    }

    public class WardOccupancy
    {
        public string WardName { get; set; } = string.Empty;
        public string WardType { get; set; } = string.Empty;
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public double OccupancyPercentage => TotalBeds > 0 ? Math.Round((double)OccupiedBeds / TotalBeds * 100, 1) : 0;
    }

    public class RecentAdmission
    {
        public int AdmissionId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string WardName { get; set; } = string.Empty;
        public string BedNumber { get; set; } = string.Empty;
        public DateTime AdmissionDate { get; set; }
        public string AdmissionType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class DoctorWorkload
    {
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int ActivePatients { get; set; }
        public bool OnDuty { get; set; }
    }

    public class MonthlyInflow
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}

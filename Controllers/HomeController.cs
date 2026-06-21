using HospitalManagement.Data;
using HospitalManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly HospitalDbContext _context;

        public HomeController(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;
            var yesterday = now.AddHours(-24);
            var monthStart = new DateTime(now.Year, now.Month, 1);

            var admissions = await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .ToListAsync();

            var wards = await _context.Wards.Include(w => w.Beds).ToListAsync();
            var doctors = await _context.Doctors.ToListAsync();

            var vm = new DashboardViewModel
            {
                TotalPatients = await _context.Patients.CountAsync(),
                ActiveAdmissions = admissions.Count(a => a.Status == "Active"),
                TotalDoctors = doctors.Count,
                DoctorsOnDuty = doctors.Count(d => d.OnDuty),
                TotalBeds = await _context.Beds.CountAsync(),
                OccupiedBeds = await _context.Beds.CountAsync(b => b.IsOccupied),
                AvailableBeds = await _context.Beds.CountAsync(b => !b.IsOccupied),
                PendingDischarges = admissions.Count(a => a.Status == "Active" && a.ExpectedDischargeDate.HasValue && a.ExpectedDischargeDate.Value.Date <= now.Date),
                EmergencyAdmissionsToday = admissions.Count(a => a.AdmissionType == "Emergency" && a.AdmissionDate.Date == now.Date),
                AdmissionsLast24Hrs = admissions.Count(a => a.AdmissionDate >= yesterday),
                DischargedThisMonth = admissions.Count(a => a.Status == "Discharged" && a.ActualDischargeDate >= monthStart),
                AverageStayDuration = admissions.Where(a => a.Status == "Discharged" && a.ActualDischargeDate.HasValue)
                    .Select(a => (a.ActualDischargeDate!.Value - a.AdmissionDate).TotalDays)
                    .DefaultIfEmpty(0).Average(),

                WardOccupancies = wards.Select(w => new WardOccupancy
                {
                    WardName = w.Name,
                    WardType = w.WardType,
                    TotalBeds = w.TotalBeds,
                    OccupiedBeds = w.Beds.Count(b => b.IsOccupied)
                }).ToList(),

                RecentAdmissions = admissions
                    .OrderByDescending(a => a.AdmissionDate)
                    .Take(10)
                    .Select(a => new RecentAdmission
                    {
                        AdmissionId = a.AdmissionId,
                        PatientName = a.Patient?.FullName ?? "N/A",
                        DoctorName = a.Doctor?.FullName ?? "N/A",
                        WardName = a.Ward?.Name ?? "N/A",
                        BedNumber = a.Bed?.BedNumber ?? "N/A",
                        AdmissionDate = a.AdmissionDate,
                        AdmissionType = a.AdmissionType,
                        Status = a.Status
                    }).ToList(),

                DoctorWorkloads = doctors.Select(d => new DoctorWorkload
                {
                    DoctorName = d.FullName,
                    Specialization = d.Specialization,
                    ActivePatients = admissions.Count(a => a.DoctorId == d.DoctorId && a.Status == "Active"),
                    OnDuty = d.OnDuty
                }).OrderByDescending(d => d.ActivePatients).Take(8).ToList(),

                MonthlyInflows = Enumerable.Range(0, 6)
                    .Select(i => now.AddMonths(-i))
                    .Select(m => new MonthlyInflow
                    {
                        Month = m.ToString("MMM yyyy"),
                        Count = admissions.Count(a => a.AdmissionDate.Year == m.Year && a.AdmissionDate.Month == m.Month)
                    })
                    .Reverse()
                    .ToList(),

                // GCN=2: average stay per ward
                WardAvgStays = wards.Select(w => new WardAvgStay
                {
                    WardName = w.Name,
                    AvgDays = admissions
                        .Where(a => a.WardId == w.WardId && a.Status == "Discharged" && a.ActualDischargeDate.HasValue)
                        .Select(a => (a.ActualDischargeDate!.Value - a.AdmissionDate).TotalDays)
                        .DefaultIfEmpty(0).Average()
                }).ToList()
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}

using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    public class PatientsController : Controller
    {
        private readonly HospitalDbContext _context;

        public PatientsController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string? search, string? gender, string? blood)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.FullName.Contains(search) || p.PhoneNumber.Contains(search));

            if (!string.IsNullOrEmpty(gender))
                query = query.Where(p => p.Gender == gender);

            if (!string.IsNullOrEmpty(blood))
                query = query.Where(p => p.BloodGroup == blood);

            ViewBag.Search = search;
            ViewBag.Gender = gender;
            ViewBag.Blood = blood;

            return View(await query.OrderByDescending(p => p.RegistrationDate).ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _context.Patients
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Doctor)
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Ward)
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Bed)
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Treatments)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null) return NotFound();
            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create() => View();

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.RegistrationDate = DateTime.Now;
                _context.Add(patient);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Patient registered successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            if (id != patient.PatientId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Patient updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Patients.Any(p => p.PatientId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Patient deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    public class TreatmentsController : Controller
    {
        private readonly HospitalDbContext _context;

        public TreatmentsController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: Treatments
        public async Task<IActionResult> Index(int? admissionId)
        {
            var query = _context.Treatments
                .Include(t => t.Admission)
                    .ThenInclude(a => a!.Patient)
                .Include(t => t.Doctor)
                .AsQueryable();

            if (admissionId.HasValue)
                query = query.Where(t => t.AdmissionId == admissionId.Value);

            ViewBag.AdmissionId = admissionId;
            return View(await query.OrderByDescending(t => t.TreatmentDate).ToListAsync());
        }

        // GET: Treatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var treatment = await _context.Treatments
                .Include(t => t.Admission).ThenInclude(a => a!.Patient)
                .Include(t => t.Doctor)
                .FirstOrDefaultAsync(t => t.TreatmentId == id);
            if (treatment == null) return NotFound();
            return View(treatment);
        }

        // GET: Treatments/Create
        public async Task<IActionResult> Create(int? admissionId)
        {
            await PopulateDropdowns(admissionId);
            var treatment = new Treatment { AdmissionId = admissionId ?? 0, TreatmentDate = DateTime.Now };
            return View(treatment);
        }

        // POST: Treatments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treatment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Treatment recorded successfully!";
                return RedirectToAction("Details", "Admissions", new { id = treatment.AdmissionId });
            }
            await PopulateDropdowns(treatment.AdmissionId);
            return View(treatment);
        }

        // GET: Treatments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment == null) return NotFound();
            await PopulateDropdowns(treatment.AdmissionId);
            return View(treatment);
        }

        // POST: Treatments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Treatment treatment)
        {
            if (id != treatment.TreatmentId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Treatment updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Treatments.Any(t => t.TreatmentId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction("Details", "Admissions", new { id = treatment.AdmissionId });
            }
            await PopulateDropdowns(treatment.AdmissionId);
            return View(treatment);
        }

        // GET: Treatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var treatment = await _context.Treatments
                .Include(t => t.Admission).ThenInclude(a => a!.Patient)
                .Include(t => t.Doctor)
                .FirstOrDefaultAsync(t => t.TreatmentId == id);
            if (treatment == null) return NotFound();
            return View(treatment);
        }

        // POST: Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            int admissionId = treatment?.AdmissionId ?? 0;
            if (treatment != null)
            {
                _context.Treatments.Remove(treatment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Treatment record deleted.";
            }
            return RedirectToAction("Details", "Admissions", new { id = admissionId });
        }

        private async Task PopulateDropdowns(int? admissionId = null)
        {
            var admissions = await _context.Admissions
                .Include(a => a.Patient)
                .Where(a => a.Status == "Active")
                .ToListAsync();
            ViewBag.Admissions = new SelectList(admissions.Select(a => new { a.AdmissionId, Label = $"{a.Patient?.FullName} - Admission #{a.AdmissionId}" }), "AdmissionId", "Label", admissionId);
            ViewBag.Doctors = new SelectList(await _context.Doctors.OrderBy(d => d.FullName).ToListAsync(), "DoctorId", "FullName");
        }
    }
}

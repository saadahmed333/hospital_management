using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    public class AdmissionsController : Controller
    {
        private readonly HospitalDbContext _context;

        public AdmissionsController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: Admissions
        public async Task<IActionResult> Index(string? status, string? type, int? wardId)
        {
            var query = _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(a => a.AdmissionType == type);

            if (wardId.HasValue)
                query = query.Where(a => a.WardId == wardId.Value);

            ViewBag.Wards = new SelectList(await _context.Wards.ToListAsync(), "WardId", "Name");
            ViewBag.Status = status;
            ViewBag.Type = type;
            ViewBag.WardId = wardId;

            return View(await query.OrderByDescending(a => a.AdmissionDate).ToListAsync());
        }

        // GET: Admissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var admission = await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .Include(a => a.Treatments)
                    .ThenInclude(t => t.Doctor)
                .FirstOrDefaultAsync(a => a.AdmissionId == id);

            if (admission == null) return NotFound();
            return View(admission);
        }

        // GET: Admissions/Create
        public async Task<IActionResult> Create(int? patientId)
        {
            await PopulateDropdowns();
            if (patientId.HasValue)
                ViewBag.PreselectedPatientId = patientId.Value;
            return View();
        }

        // POST: Admissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admission admission)
        {
            // Validate bed not already occupied
            var bed = await _context.Beds.FindAsync(admission.BedId);
            if (bed == null || bed.IsOccupied)
            {
                ModelState.AddModelError("BedId", "The selected bed is not available.");
            }

            // Validate no duplicate active admission for patient
            var existingAdmission = await _context.Admissions
                .AnyAsync(a => a.PatientId == admission.PatientId && a.Status == "Active");
            if (existingAdmission)
            {
                ModelState.AddModelError("PatientId", "This patient already has an active admission.");
            }

            if (ModelState.IsValid)
            {
                admission.AdmissionDate = DateTime.Now;
                admission.Status = "Active";
                _context.Add(admission);

                // Mark bed as occupied
                if (bed != null)
                {
                    bed.IsOccupied = true;
                    _context.Update(bed);
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Patient admitted successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(admission);
            return View(admission);
        }

        // GET: Admissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var admission = await _context.Admissions.FindAsync(id);
            if (admission == null) return NotFound();
            await PopulateDropdowns(admission);
            return View(admission);
        }

        // POST: Admissions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Admission admission)
        {
            if (id != admission.AdmissionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admission);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Admission updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Admissions.Any(a => a.AdmissionId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns(admission);
            return View(admission);
        }

        // GET: Admissions/Discharge/5
        public async Task<IActionResult> Discharge(int? id)
        {
            if (id == null) return NotFound();
            var admission = await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Bed)
                .Include(a => a.Ward)
                .FirstOrDefaultAsync(a => a.AdmissionId == id);
            if (admission == null || admission.Status != "Active") return NotFound();
            return View(admission);
        }

        // POST: Admissions/Discharge/5
        [HttpPost, ActionName("Discharge")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DischargeConfirmed(int id, string? dischargeNotes, string? diagnosis)
        {
            var admission = await _context.Admissions.Include(a => a.Bed).FirstOrDefaultAsync(a => a.AdmissionId == id);
            if (admission == null) return NotFound();

            admission.Status = "Discharged";
            admission.ActualDischargeDate = DateTime.Now;
            admission.DischargeNotes = dischargeNotes;
            if (!string.IsNullOrEmpty(diagnosis))
                admission.Diagnosis = diagnosis;

            // Free up the bed
            if (admission.Bed != null)
            {
                admission.Bed.IsOccupied = false;
                _context.Update(admission.Bed);
            }

            _context.Update(admission);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Patient discharged successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var admission = await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .FirstOrDefaultAsync(a => a.AdmissionId == id);
            if (admission == null) return NotFound();
            return View(admission);
        }

        // POST: Admissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admission = await _context.Admissions.Include(a => a.Bed).FirstOrDefaultAsync(a => a.AdmissionId == id);
            if (admission != null)
            {
                if (admission.Bed != null && admission.Status == "Active")
                {
                    admission.Bed.IsOccupied = false;
                    _context.Update(admission.Bed);
                }
                _context.Admissions.Remove(admission);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Admission record deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // AJAX: Get available beds for ward
        [HttpGet]
        public async Task<IActionResult> GetAvailableBeds(int wardId)
        {
            var beds = await _context.Beds
                .Where(b => b.WardId == wardId && !b.IsOccupied)
                .Select(b => new { b.BedId, b.BedNumber, b.BedType })
                .ToListAsync();
            return Json(beds);
        }

        private async Task PopulateDropdowns(Admission? admission = null)
        {
            ViewBag.Patients = new SelectList(await _context.Patients.OrderBy(p => p.FullName).ToListAsync(), "PatientId", "FullName", admission?.PatientId);
            ViewBag.Doctors = new SelectList(await _context.Doctors.Where(d => d.IsAvailable).OrderBy(d => d.FullName).ToListAsync(), "DoctorId", "FullName", admission?.DoctorId);
            ViewBag.Wards = new SelectList(await _context.Wards.ToListAsync(), "WardId", "Name", admission?.WardId);
            if (admission?.WardId > 0)
                ViewBag.Beds = new SelectList(await _context.Beds.Where(b => b.WardId == admission.WardId && (!b.IsOccupied || b.BedId == admission.BedId)).ToListAsync(), "BedId", "BedNumber", admission.BedId);
            else
                ViewBag.Beds = new SelectList(Enumerable.Empty<object>(), "BedId", "BedNumber");
        }
    }
}

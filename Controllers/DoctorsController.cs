using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly HospitalDbContext _context;

        public DoctorsController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string? search, string? wardType, bool? available)
        {
            var query = _context.Doctors.Include(d => d.Ward).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(d => d.FullName.Contains(search) || d.Specialization.Contains(search));

            if (!string.IsNullOrEmpty(wardType))
                query = query.Where(d => d.Ward!.WardType == wardType);

            if (available.HasValue)
                query = query.Where(d => d.IsAvailable == available.Value);

            ViewBag.Search = search;
            ViewBag.WardType = wardType;
            ViewBag.Available = available;

            return View(await query.OrderBy(d => d.FullName).ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .Include(d => d.Ward)
                .Include(d => d.Admissions)
                    .ThenInclude(a => a.Patient)
                .Include(d => d.Treatments)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // GET: Doctors/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Wards = new SelectList(await _context.Wards.ToListAsync(), "WardId", "Name");
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Doctor added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Wards = new SelectList(await _context.Wards.ToListAsync(), "WardId", "Name", doctor.WardId);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();
            ViewBag.Wards = new SelectList(await _context.Wards.ToListAsync(), "WardId", "Name", doctor.WardId);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Doctor updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doctors.Any(d => d.DoctorId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Wards = new SelectList(await _context.Wards.ToListAsync(), "WardId", "Name", doctor.WardId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var doctor = await _context.Doctors.Include(d => d.Ward).FirstOrDefaultAsync(d => d.DoctorId == id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Doctor removed successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Toggle Duty Status
        [HttpPost]
        public async Task<IActionResult> ToggleDuty(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                doctor.OnDuty = !doctor.OnDuty;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

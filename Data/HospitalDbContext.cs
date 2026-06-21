using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }

        public DbSet<Ward> Wards { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ward configuration
            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasKey(w => w.WardId);
                entity.Property(w => w.Name).IsRequired().HasMaxLength(100);
                entity.Property(w => w.WardType).IsRequired().HasMaxLength(50);
            });

            // Bed configuration
            modelBuilder.Entity<Bed>(entity =>
            {
                entity.HasKey(b => b.BedId);
                entity.HasOne(b => b.Ward)
                      .WithMany(w => w.Beds)
                      .HasForeignKey(b => b.WardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Doctor configuration
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.DoctorId);
                entity.HasOne(d => d.Ward)
                      .WithMany(w => w.Doctors)
                      .HasForeignKey(d => d.WardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.PatientId);
            });

            // Admission configuration
            modelBuilder.Entity<Admission>(entity =>
            {
                entity.HasKey(a => a.AdmissionId);
                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Admissions)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.Admissions)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Ward)
                      .WithMany(w => w.Admissions)
                      .HasForeignKey(a => a.WardId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Bed)
                      .WithMany(b => b.Admissions)
                      .HasForeignKey(a => a.BedId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Treatment configuration
            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.HasKey(t => t.TreatmentId);
                entity.HasOne(t => t.Admission)
                      .WithMany(a => a.Treatments)
                      .HasForeignKey(t => t.AdmissionId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(t => t.Doctor)
                      .WithMany(d => d.Treatments)
                      .HasForeignKey(t => t.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Wards
            modelBuilder.Entity<Ward>().HasData(
                new Ward { WardId = 1, Name = "General Ward", WardType = "General", TotalBeds = 30, Description = "General patient care ward for routine treatments" },
                new Ward { WardId = 2, Name = "Intensive Care Unit", WardType = "ICU", TotalBeds = 15, Description = "Critical care unit for life-threatening conditions" },
                new Ward { WardId = 3, Name = "Maternity Ward", WardType = "Maternity", TotalBeds = 20, Description = "Dedicated ward for maternity and newborn care" },
                new Ward { WardId = 4, Name = "Pediatric Ward", WardType = "Pediatric", TotalBeds = 25, Description = "Specialized care for children and infants" }
            );

            // Seed Beds for General Ward (1-30)
            var beds = new List<Bed>();
            int bedId = 1;
            for (int i = 1; i <= 30; i++)
                beds.Add(new Bed { BedId = bedId++, BedNumber = $"G-{i:D2}", WardId = 1, BedType = "Standard", IsOccupied = false });
            for (int i = 1; i <= 15; i++)
                beds.Add(new Bed { BedId = bedId++, BedNumber = $"ICU-{i:D2}", WardId = 2, BedType = "ICU", IsOccupied = false });
            for (int i = 1; i <= 20; i++)
                beds.Add(new Bed { BedId = bedId++, BedNumber = $"M-{i:D2}", WardId = 3, BedType = "Standard", IsOccupied = false });
            for (int i = 1; i <= 25; i++)
                beds.Add(new Bed { BedId = bedId++, BedNumber = $"P-{i:D2}", WardId = 4, BedType = "Pediatric", IsOccupied = false });
            modelBuilder.Entity<Bed>().HasData(beds);

            // Seed Doctors
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, FullName = "Dr. Ahmed Khan", Specialization = "General Medicine", Email = "ahmed.khan@medicore.pk", PhoneNumber = "0321-1234567", WardId = 1, IsAvailable = true, OnDuty = true, ConsultationSchedule = "Mon-Fri 9AM-5PM", YearsExperience = 12, JoinedDate = new DateTime(2015, 3, 10) },
                new Doctor { DoctorId = 2, FullName = "Dr. Sara Ali", Specialization = "Cardiology", Email = "sara.ali@medicore.pk", PhoneNumber = "0322-2345678", WardId = 2, IsAvailable = true, OnDuty = true, ConsultationSchedule = "Mon-Sat 8AM-4PM", YearsExperience = 15, JoinedDate = new DateTime(2012, 7, 1) },
                new Doctor { DoctorId = 3, FullName = "Dr. Fatima Raza", Specialization = "Obstetrics & Gynecology", Email = "fatima.raza@medicore.pk", PhoneNumber = "0323-3456789", WardId = 3, IsAvailable = true, OnDuty = false, ConsultationSchedule = "Tue-Sat 10AM-6PM", YearsExperience = 10, JoinedDate = new DateTime(2016, 1, 15) },
                new Doctor { DoctorId = 4, FullName = "Dr. Usman Sheikh", Specialization = "Pediatrics", Email = "usman.sheikh@medicore.pk", PhoneNumber = "0324-4567890", WardId = 4, IsAvailable = false, OnDuty = false, ConsultationSchedule = "Mon-Thu 9AM-3PM", YearsExperience = 8, JoinedDate = new DateTime(2018, 5, 20) },
                new Doctor { DoctorId = 5, FullName = "Dr. Zara Hussain", Specialization = "Neurology", Email = "zara.hussain@medicore.pk", PhoneNumber = "0325-5678901", WardId = 1, IsAvailable = true, OnDuty = true, ConsultationSchedule = "Mon-Fri 11AM-7PM", YearsExperience = 18, JoinedDate = new DateTime(2010, 9, 5) },
                new Doctor { DoctorId = 6, FullName = "Dr. Bilal Malik", Specialization = "Orthopedics", Email = "bilal.malik@medicore.pk", PhoneNumber = "0326-6789012", WardId = 1, IsAvailable = true, OnDuty = false, ConsultationSchedule = "Wed-Sun 8AM-2PM", YearsExperience = 14, JoinedDate = new DateTime(2013, 11, 30) },
                new Doctor { DoctorId = 7, FullName = "Dr. Hina Baig", Specialization = "Internal Medicine", Email = "hina.baig@medicore.pk", PhoneNumber = "0327-7890123", WardId = 2, IsAvailable = true, OnDuty = true, ConsultationSchedule = "Mon-Fri 7AM-3PM", YearsExperience = 9, JoinedDate = new DateTime(2017, 4, 12) },
                new Doctor { DoctorId = 8, FullName = "Dr. Kamran Iqbal", Specialization = "Dermatology", Email = "kamran.iqbal@medicore.pk", PhoneNumber = "0328-8901234", WardId = 4, IsAvailable = false, OnDuty = true, ConsultationSchedule = "Tue-Sat 12PM-8PM", YearsExperience = 7, JoinedDate = new DateTime(2019, 8, 3) }
            );

            // Seed Patients
            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, FullName = "Mohammad Asif", DateOfBirth = new DateTime(1980, 5, 15), Gender = "Male", BloodGroup = "B+", PhoneNumber = "0301-1112223", Email = "asif@gmail.com", Address = "House 12, Block A, Gulshan-e-Iqbal, Karachi", EmergencyContactName = "Ayesha Asif", EmergencyContactPhone = "0301-9998887", MedicalHistory = "Hypertension, Diabetes Type 2", KnownAllergies = "Penicillin", RegistrationDate = new DateTime(2024, 1, 10) },
                new Patient { PatientId = 2, FullName = "Nadia Farrukh", DateOfBirth = new DateTime(1995, 8, 22), Gender = "Female", BloodGroup = "O+", PhoneNumber = "0302-2223334", Address = "Flat 5, Tower B, DHA Phase 6, Karachi", EmergencyContactName = "Tariq Farrukh", EmergencyContactPhone = "0302-6667778", MedicalHistory = "Asthma", KnownAllergies = "None", RegistrationDate = new DateTime(2024, 2, 20) },
                new Patient { PatientId = 3, FullName = "Rizwan Qureshi", DateOfBirth = new DateTime(1965, 3, 10), Gender = "Male", BloodGroup = "A-", PhoneNumber = "0303-3334445", Address = "Plot 8, Street 3, North Nazimabad, Karachi", EmergencyContactName = "Sana Qureshi", EmergencyContactPhone = "0303-4445556", MedicalHistory = "Cardiac arrhythmia, Hypertension", KnownAllergies = "Sulfa drugs", RegistrationDate = new DateTime(2024, 3, 5) },
                new Patient { PatientId = 4, FullName = "Amna Siddiqui", DateOfBirth = new DateTime(1990, 11, 30), Gender = "Female", BloodGroup = "AB+", PhoneNumber = "0304-4445556", Address = "House 45, Bahadurabad, Karachi", EmergencyContactName = "Omar Siddiqui", EmergencyContactPhone = "0304-1112223", RegistrationDate = new DateTime(2024, 4, 15) },
                new Patient { PatientId = 5, FullName = "Ali Hassan", DateOfBirth = new DateTime(2018, 7, 5), Gender = "Male", BloodGroup = "O-", PhoneNumber = "0305-5556667", Address = "Flat 2, Model Colony, Karachi", EmergencyContactName = "Hassan Ali", EmergencyContactPhone = "0305-7778889", MedicalHistory = "Recurrent fever episodes", RegistrationDate = new DateTime(2024, 5, 3) },
                new Patient { PatientId = 6, FullName = "Sobia Nawaz", DateOfBirth = new DateTime(1985, 12, 18), Gender = "Female", BloodGroup = "A+", PhoneNumber = "0306-6667778", Address = "House 78, Gulberg Town, Karachi", EmergencyContactName = "Imran Nawaz", EmergencyContactPhone = "0306-2223334", MedicalHistory = "Pregnancy - 32 weeks", RegistrationDate = new DateTime(2024, 6, 10) }
            );
        }
    }
}

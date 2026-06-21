using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Gender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BloodGroup = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmergencyContactName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmergencyContactPhone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicalHistory = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KnownAllergies = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    WardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WardType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalBeds = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.WardId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Beds",
                columns: table => new
                {
                    BedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BedNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    IsOccupied = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BedType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.BedId);
                    table.ForeignKey(
                        name: "FK_Beds_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "WardId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Specialization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConsultationSchedule = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YearsExperience = table.Column<int>(type: "int", nullable: false),
                    OnDuty = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                    table.ForeignKey(
                        name: "FK_Doctors_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "WardId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    AdmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    BedId = table.Column<int>(type: "int", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpectedDischargeDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ActualDischargeDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AdmissionType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChiefComplaint = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Diagnosis = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DischargeNotes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.AdmissionId);
                    table.ForeignKey(
                        name: "FK_Admissions_Beds_BedId",
                        column: x => x.BedId,
                        principalTable: "Beds",
                        principalColumn: "BedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "WardId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    TreatmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdmissionId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    TreatmentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TreatmentType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Medication = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TreatmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.TreatmentId);
                    table.ForeignKey(
                        name: "FK_Treatments_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "AdmissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treatments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "Address", "BloodGroup", "DateOfBirth", "Email", "EmergencyContactName", "EmergencyContactPhone", "FullName", "Gender", "KnownAllergies", "MedicalHistory", "PhoneNumber", "RegistrationDate" },
                values: new object[,]
                {
                    { 1, "House 12, Block A, Gulshan-e-Iqbal, Karachi", "B+", new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "asif@gmail.com", "Ayesha Asif", "0301-9998887", "Mohammad Asif", "Male", "Penicillin", "Hypertension, Diabetes Type 2", "0301-1112223", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Flat 5, Tower B, DHA Phase 6, Karachi", "O+", new DateTime(1995, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tariq Farrukh", "0302-6667778", "Nadia Farrukh", "Female", "None", "Asthma", "0302-2223334", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Plot 8, Street 3, North Nazimabad, Karachi", "A-", new DateTime(1965, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sana Qureshi", "0303-4445556", "Rizwan Qureshi", "Male", "Sulfa drugs", "Cardiac arrhythmia, Hypertension", "0303-3334445", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "House 45, Bahadurabad, Karachi", "AB+", new DateTime(1990, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Omar Siddiqui", "0304-1112223", "Amna Siddiqui", "Female", null, null, "0304-4445556", new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Flat 2, Model Colony, Karachi", "O-", new DateTime(2018, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hassan Ali", "0305-7778889", "Ali Hassan", "Male", null, "Recurrent fever episodes", "0305-5556667", new DateTime(2024, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "House 78, Gulberg Town, Karachi", "A+", new DateTime(1985, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Imran Nawaz", "0306-2223334", "Sobia Nawaz", "Female", null, "Pregnancy - 32 weeks", "0306-6667778", new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Wards",
                columns: new[] { "WardId", "Description", "Name", "TotalBeds", "WardType" },
                values: new object[,]
                {
                    { 1, "General patient care ward for routine treatments", "General Ward", 30, "General" },
                    { 2, "Critical care unit for life-threatening conditions", "Intensive Care Unit", 15, "ICU" },
                    { 3, "Dedicated ward for maternity and newborn care", "Maternity Ward", 20, "Maternity" },
                    { 4, "Specialized care for children and infants", "Pediatric Ward", 25, "Pediatric" }
                });

            migrationBuilder.InsertData(
                table: "Beds",
                columns: new[] { "BedId", "BedNumber", "BedType", "IsOccupied", "WardId" },
                values: new object[,]
                {
                    { 1, "G-01", "Standard", false, 1 },
                    { 2, "G-02", "Standard", false, 1 },
                    { 3, "G-03", "Standard", false, 1 },
                    { 4, "G-04", "Standard", false, 1 },
                    { 5, "G-05", "Standard", false, 1 },
                    { 6, "G-06", "Standard", false, 1 },
                    { 7, "G-07", "Standard", false, 1 },
                    { 8, "G-08", "Standard", false, 1 },
                    { 9, "G-09", "Standard", false, 1 },
                    { 10, "G-10", "Standard", false, 1 },
                    { 11, "G-11", "Standard", false, 1 },
                    { 12, "G-12", "Standard", false, 1 },
                    { 13, "G-13", "Standard", false, 1 },
                    { 14, "G-14", "Standard", false, 1 },
                    { 15, "G-15", "Standard", false, 1 },
                    { 16, "G-16", "Standard", false, 1 },
                    { 17, "G-17", "Standard", false, 1 },
                    { 18, "G-18", "Standard", false, 1 },
                    { 19, "G-19", "Standard", false, 1 },
                    { 20, "G-20", "Standard", false, 1 },
                    { 21, "G-21", "Standard", false, 1 },
                    { 22, "G-22", "Standard", false, 1 },
                    { 23, "G-23", "Standard", false, 1 },
                    { 24, "G-24", "Standard", false, 1 },
                    { 25, "G-25", "Standard", false, 1 },
                    { 26, "G-26", "Standard", false, 1 },
                    { 27, "G-27", "Standard", false, 1 },
                    { 28, "G-28", "Standard", false, 1 },
                    { 29, "G-29", "Standard", false, 1 },
                    { 30, "G-30", "Standard", false, 1 },
                    { 31, "ICU-01", "ICU", false, 2 },
                    { 32, "ICU-02", "ICU", false, 2 },
                    { 33, "ICU-03", "ICU", false, 2 },
                    { 34, "ICU-04", "ICU", false, 2 },
                    { 35, "ICU-05", "ICU", false, 2 },
                    { 36, "ICU-06", "ICU", false, 2 },
                    { 37, "ICU-07", "ICU", false, 2 },
                    { 38, "ICU-08", "ICU", false, 2 },
                    { 39, "ICU-09", "ICU", false, 2 },
                    { 40, "ICU-10", "ICU", false, 2 },
                    { 41, "ICU-11", "ICU", false, 2 },
                    { 42, "ICU-12", "ICU", false, 2 },
                    { 43, "ICU-13", "ICU", false, 2 },
                    { 44, "ICU-14", "ICU", false, 2 },
                    { 45, "ICU-15", "ICU", false, 2 },
                    { 46, "M-01", "Standard", false, 3 },
                    { 47, "M-02", "Standard", false, 3 },
                    { 48, "M-03", "Standard", false, 3 },
                    { 49, "M-04", "Standard", false, 3 },
                    { 50, "M-05", "Standard", false, 3 },
                    { 51, "M-06", "Standard", false, 3 },
                    { 52, "M-07", "Standard", false, 3 },
                    { 53, "M-08", "Standard", false, 3 },
                    { 54, "M-09", "Standard", false, 3 },
                    { 55, "M-10", "Standard", false, 3 },
                    { 56, "M-11", "Standard", false, 3 },
                    { 57, "M-12", "Standard", false, 3 },
                    { 58, "M-13", "Standard", false, 3 },
                    { 59, "M-14", "Standard", false, 3 },
                    { 60, "M-15", "Standard", false, 3 },
                    { 61, "M-16", "Standard", false, 3 },
                    { 62, "M-17", "Standard", false, 3 },
                    { 63, "M-18", "Standard", false, 3 },
                    { 64, "M-19", "Standard", false, 3 },
                    { 65, "M-20", "Standard", false, 3 },
                    { 66, "P-01", "Pediatric", false, 4 },
                    { 67, "P-02", "Pediatric", false, 4 },
                    { 68, "P-03", "Pediatric", false, 4 },
                    { 69, "P-04", "Pediatric", false, 4 },
                    { 70, "P-05", "Pediatric", false, 4 },
                    { 71, "P-06", "Pediatric", false, 4 },
                    { 72, "P-07", "Pediatric", false, 4 },
                    { 73, "P-08", "Pediatric", false, 4 },
                    { 74, "P-09", "Pediatric", false, 4 },
                    { 75, "P-10", "Pediatric", false, 4 },
                    { 76, "P-11", "Pediatric", false, 4 },
                    { 77, "P-12", "Pediatric", false, 4 },
                    { 78, "P-13", "Pediatric", false, 4 },
                    { 79, "P-14", "Pediatric", false, 4 },
                    { 80, "P-15", "Pediatric", false, 4 },
                    { 81, "P-16", "Pediatric", false, 4 },
                    { 82, "P-17", "Pediatric", false, 4 },
                    { 83, "P-18", "Pediatric", false, 4 },
                    { 84, "P-19", "Pediatric", false, 4 },
                    { 85, "P-20", "Pediatric", false, 4 },
                    { 86, "P-21", "Pediatric", false, 4 },
                    { 87, "P-22", "Pediatric", false, 4 },
                    { 88, "P-23", "Pediatric", false, 4 },
                    { 89, "P-24", "Pediatric", false, 4 },
                    { 90, "P-25", "Pediatric", false, 4 }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationSchedule", "Email", "FullName", "IsAvailable", "JoinedDate", "OnDuty", "PhoneNumber", "Specialization", "WardId", "YearsExperience" },
                values: new object[,]
                {
                    { 1, "Mon-Fri 9AM-5PM", "ahmed.khan@medicore.pk", "Dr. Ahmed Khan", true, new DateTime(2015, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0321-1234567", "General Medicine", 1, 12 },
                    { 2, "Mon-Sat 8AM-4PM", "sara.ali@medicore.pk", "Dr. Sara Ali", true, new DateTime(2012, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0322-2345678", "Cardiology", 2, 15 },
                    { 3, "Tue-Sat 10AM-6PM", "fatima.raza@medicore.pk", "Dr. Fatima Raza", true, new DateTime(2016, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "0323-3456789", "Obstetrics & Gynecology", 3, 10 },
                    { 4, "Mon-Thu 9AM-3PM", "usman.sheikh@medicore.pk", "Dr. Usman Sheikh", false, new DateTime(2018, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "0324-4567890", "Pediatrics", 4, 8 },
                    { 5, "Mon-Fri 11AM-7PM", "zara.hussain@medicore.pk", "Dr. Zara Hussain", true, new DateTime(2010, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0325-5678901", "Neurology", 1, 18 },
                    { 6, "Wed-Sun 8AM-2PM", "bilal.malik@medicore.pk", "Dr. Bilal Malik", true, new DateTime(2013, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "0326-6789012", "Orthopedics", 1, 14 },
                    { 7, "Mon-Fri 7AM-3PM", "hina.baig@medicore.pk", "Dr. Hina Baig", true, new DateTime(2017, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0327-7890123", "Internal Medicine", 2, 9 },
                    { 8, "Tue-Sat 12PM-8PM", "kamran.iqbal@medicore.pk", "Dr. Kamran Iqbal", false, new DateTime(2019, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0328-8901234", "Dermatology", 4, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_BedId",
                table: "Admissions",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DoctorId",
                table: "Admissions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_WardId",
                table: "Admissions",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_WardId",
                table: "Beds",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_WardId",
                table: "Doctors",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_AdmissionId",
                table: "Treatments",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DoctorId",
                table: "Treatments",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.DropTable(
                name: "Beds");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Wards");
        }
    }
}

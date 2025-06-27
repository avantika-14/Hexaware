namespace HospitalDBManagement.Migrations
{
    using HospitalManagementSystem.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HospitalManagementSystem.Entities.HospitalDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HospitalManagementSystem.Entities.HospitalDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            if (!context.Patients.Any())
            {
                context.Patients.AddRange(new[]
                {
                    new Patient { FirstName = "Alice", LastName = "Thomas", DateOfBirth = new DateTime(1985, 1, 12), Gender = "Female", ContactNumber = "9876543210", Address = "Mumbai" },
                    new Patient { FirstName = "Bob", LastName = "Singh", DateOfBirth = new DateTime(1990, 5, 25), Gender = "Male", ContactNumber = "8765432109", Address = "Delhi" },
                    new Patient { FirstName = "Clara", LastName = "Fernandez", DateOfBirth = new DateTime(1998, 9, 10), Gender = "Female", ContactNumber = "7654321098", Address = "Chennai" }
                });
            }

            if (!context.Doctors.Any())
            {
                context.Doctors.AddRange(new[]
                {
                    new Doctor { FirstName = "David", LastName = "Wong", Specialization = "Cardiology", ContactNumber = "9998887770" },
                    new Doctor { FirstName = "Emily", LastName = "Clark", Specialization = "Dermatology", ContactNumber = "9998887771" }
                });
            }

            context.SaveChanges(); // To get IDs assigned

            if (!context.Appointments.Any())
            {
                var patient1 = context.Patients.First(p => p.FirstName == "Alice");
                var patient2 = context.Patients.First(p => p.FirstName == "Bob");
                var doctor1 = context.Doctors.First(d => d.FirstName == "David");
                var doctor2 = context.Doctors.First(d => d.FirstName == "Emily");

                context.Appointments.AddRange(new[]
                {
                    new Appointment { PatientId = patient1.PatientId, DoctorId = doctor1.DoctorId, AppointmentDate = new DateTime(2025, 7, 5, 10, 30, 0), Description = "Heart check-up" },
                    new Appointment { PatientId = patient2.PatientId, DoctorId = doctor2.DoctorId, AppointmentDate = new DateTime(2025, 7, 6, 15, 0, 0), Description = "Skin allergy consultation" }
                });
            }

            context.SaveChanges();
        }
    }
}

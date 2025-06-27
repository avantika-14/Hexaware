using HospitalDBManagement.DAO;
using HospitalDBManagement.Exceptions;
using HospitalManagementSystem.Entities;
using System;
using System.Collections.Generic;

namespace HospitalDBManagement.Main
{
    class Program
    {
        private static readonly IHospitalServices hospitalService = new HospitalServicesImplementation();

        static void Main(string[] args)
        {
            Console.WriteLine("Hospital Management System");

            while (true)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            GetAppointmentById();
                            break;
                        case "2":
                            GetAppointmentsForPatient();
                            break;
                        case "3":
                            GetAppointmentsForDoctor();
                            break;
                        case "4":
                            ScheduleAppointment();
                            break;
                        case "5":
                            UpdateAppointment();
                            break;
                        case "6":
                            CancelAppointment();
                            break;
                        case "7":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (PatientNumberNotFoundException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Get Appointment by ID");
            Console.WriteLine("2. Get Appointments for Patient");
            Console.WriteLine("3. Get Appointments for Doctor");
            Console.WriteLine("4. Schedule Appointment");
            Console.WriteLine("5. Update Appointment");
            Console.WriteLine("6. Cancel Appointment");
            Console.WriteLine("7. Exit");
            Console.Write("Enter your choice (1-7): ");
        }

        static void GetAppointmentById()
        {
            Console.Write("Enter Appointment ID: ");
            int id = int.Parse(Console.ReadLine());

            var appointment = hospitalService.GetAppointmentById(id);
            if (appointment != null)
            {
                Console.WriteLine($"\nAppointment Details:");
                Console.WriteLine($"ID: {appointment.AppointmentId}");
                Console.WriteLine($"Date: {appointment.AppointmentDate}");
                Console.WriteLine($"Patient ID: {appointment.PatientId}");
                Console.WriteLine($"Doctor ID: {appointment.DoctorId}");
                Console.WriteLine($"Description: {appointment.Description}");
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }

        static void GetAppointmentsForPatient()
        {
            Console.Write("Enter Patient ID: ");
            int patientId = int.Parse(Console.ReadLine());

            var appointments = hospitalService.GetAppointmentsForPatient(patientId);
            DisplayAppointments(appointments, $"for Patient ID: {patientId}");
        }

        static void GetAppointmentsForDoctor()
        {
            Console.Write("Enter Doctor ID: ");
            int doctorId = int.Parse(Console.ReadLine());

            var appointments = hospitalService.GetAppointmentsForDoctor(doctorId);
            DisplayAppointments(appointments, $"for Doctor ID: {doctorId}");
        }

        static void ScheduleAppointment()
        {
            var appointment = new Appointment();

            Console.WriteLine("\nSchedule New Appointment");
            Console.Write("Enter Patient ID: ");
            appointment.PatientId = int.Parse(Console.ReadLine());

            Console.Write("Enter Doctor ID: ");
            appointment.DoctorId = int.Parse(Console.ReadLine());

            Console.Write("Enter Appointment Date (yyyy-MM-dd HH:mm): ");
            appointment.AppointmentDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Description (optional): ");
            appointment.Description = Console.ReadLine();

            bool success = hospitalService.ScheduleAppointment(appointment);
            Console.WriteLine(success ? "Appointment scheduled successfully!" : "Failed to schedule appointment.");
        }

        static void UpdateAppointment()
        {
            Console.Write("Enter Appointment ID to update: ");
            int id = int.Parse(Console.ReadLine());

            var existing = hospitalService.GetAppointmentById(id);
            if (existing == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            Console.WriteLine("\nCurrent Appointment Details:");
            Console.WriteLine($"Date: {existing.AppointmentDate}");
            Console.WriteLine($"Patient ID: {existing.PatientId}");
            Console.WriteLine($"Doctor ID: {existing.DoctorId}");
            Console.WriteLine($"Description: {existing.Description}");

            var updated = new Appointment { AppointmentId = id };

            Console.Write("\nEnter new Patient ID (leave blank to keep current): ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
                updated.PatientId = patientId;
            else
                updated.PatientId = existing.PatientId;

            Console.Write("Enter new Doctor ID (leave blank to keep current): ");
            if (int.TryParse(Console.ReadLine(), out int doctorId))
                updated.DoctorId = doctorId;
            else
                updated.DoctorId = existing.DoctorId;

            Console.Write("Enter new Date (yyyy-MM-dd HH:mm, leave blank to keep current): ");
            var dateInput = Console.ReadLine();
            updated.AppointmentDate = !string.IsNullOrEmpty(dateInput)
                ? DateTime.Parse(dateInput)
                : existing.AppointmentDate;

            Console.Write("Enter new Description (leave blank to keep current): ");
            updated.Description = Console.ReadLine();
            if (string.IsNullOrEmpty(updated.Description))
                updated.Description = existing.Description;

            bool success = hospitalService.UpdateAppointment(updated);
            Console.WriteLine(success ? "Appointment updated successfully!" : "Failed to update appointment.");
        }

        static void CancelAppointment()
        {
            Console.Write("Enter Appointment ID to cancel: ");
            int id = int.Parse(Console.ReadLine());

            bool success = hospitalService.CancelAppointment(id);
            Console.WriteLine(success ? "Appointment cancelled successfully!" : "Failed to cancel appointment.");
        }

        static void DisplayAppointments(List<Appointment> appointments, string filterInfo)
        {
            Console.WriteLine($"\nFound {appointments.Count} appointments {filterInfo}:");

            foreach (var app in appointments)
            {
                Console.WriteLine($"\nID: {app.AppointmentId}");
                Console.WriteLine($"Date: {app.AppointmentDate}");
                Console.WriteLine($"Patient: {app.PatientId}");
                Console.WriteLine($"Doctor: {app.DoctorId}");
                Console.WriteLine($"Description: {app.Description}");
                Console.WriteLine("-------------------");
            }
        }
    }
}
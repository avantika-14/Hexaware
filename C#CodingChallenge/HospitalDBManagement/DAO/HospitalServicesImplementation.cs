using HospitalDBManagement.Exceptions;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalDBManagement.DAO
{
    public class HospitalServicesImplementation : IHospitalServices
    {
        private readonly string connectionString;

        public HospitalServicesImplementation()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("SELECT * FROM Appointments WHERE AppointmentId = @AppointmentId", connection))
            {
                command.Parameters.AddWithValue("@AppointmentId", appointmentId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Appointment
                        {
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            DoctorId = Convert.ToInt32(reader["DoctorId"]),
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                            Description = reader["Description"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public List<Appointment> GetAppointmentsForPatient(int patientId)
        {
            var appointments = new List<Appointment>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("SELECT * FROM Appointments WHERE PatientId = @PatientId", connection))
            {
                command.Parameters.AddWithValue("@PatientId", patientId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            DoctorId = Convert.ToInt32(reader["DoctorId"]),
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }

            if (appointments.Count == 0)
            {
                throw new PatientNumberNotFoundException(patientId);
            }

            return appointments;
        }

        public List<Appointment> GetAppointmentsForDoctor(int doctorId)
        {
            var appointments = new List<Appointment>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("SELECT * FROM Appointments WHERE DoctorId = @DoctorId", connection))
            {
                command.Parameters.AddWithValue("@DoctorId", doctorId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            DoctorId = Convert.ToInt32(reader["DoctorId"]),
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }

            return appointments;
        }

        public bool ScheduleAppointment(Appointment appointment)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                "INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, Description) " +
                "VALUES (@PatientId, @DoctorId, @AppointmentDate, @Description);", connection))
            {
                command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@Description", appointment.Description ?? (object)DBNull.Value);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UpdateAppointment(Appointment appointment)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                "UPDATE Appointments SET " +
                "PatientId = @PatientId, " +
                "DoctorId = @DoctorId, " +
                "AppointmentDate = @AppointmentDate, " +
                "Description = @Description " +
                "WHERE AppointmentId = @AppointmentId", connection))
            {
                command.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
                command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@Description", appointment.Description ?? (object)DBNull.Value);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool CancelAppointment(int appointmentId)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                "DELETE FROM Appointments WHERE AppointmentId = @AppointmentId", connection))
            {
                command.Parameters.AddWithValue("@AppointmentId", appointmentId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        // Foreign keys
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        // Navigation properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }

        public Appointment() { }

        public Appointment(int patientId, int doctorId, DateTime appointmentDate, string description)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            Description = description;
        }

        public override string ToString()
        {
            return $"AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}, Date: {AppointmentDate}, Description: {Description}";
        }
    }
}

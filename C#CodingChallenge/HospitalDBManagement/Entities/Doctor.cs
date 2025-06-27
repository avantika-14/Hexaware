using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string ContactNumber { get; set; }

        public Doctor() { }

        public Doctor(string firstName, string lastName, string specialization, string contactNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Specialization = specialization;
            ContactNumber = contactNumber;
        }

        public override string ToString()
        {
            return $"DoctorId: {DoctorId}, Name: {FirstName} {LastName}, Specialization: {Specialization}, Contact: {ContactNumber}";
        }
    }
}

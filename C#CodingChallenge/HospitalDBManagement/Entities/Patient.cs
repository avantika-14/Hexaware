using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }

        public Patient() { }

        public Patient(string firstName, string lastName, DateTime dateOfBirth, string gender, string contactNumber, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            ContactNumber = contactNumber;
            Address = address;
        }

        public override string ToString()
        {
            return $"PatientId: {PatientId}, Name: {FirstName} {LastName}, DOB: {DateOfBirth.ToShortDateString()}, Gender: {Gender}, Contact: {ContactNumber}, Address: {Address}";
        }
    }
}

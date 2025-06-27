using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalDBManagement.Exceptions
{
    public class PatientNumberNotFoundException : Exception
    {
        public PatientNumberNotFoundException(int id) : base($"Patient with ID {id} not found.") { }
    }
}

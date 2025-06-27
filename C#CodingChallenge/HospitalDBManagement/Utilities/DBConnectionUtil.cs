using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Utilities
{
    public class DBConnectionUtil
    {
        public static SqlConnection GetConnection()
        {
            string connStr = DBPropertyUtil.GetConnectionString();
            return new SqlConnection(connStr);
        }
    }
}

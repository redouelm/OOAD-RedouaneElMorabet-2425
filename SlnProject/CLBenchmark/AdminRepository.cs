using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace CLBenchmark
{
    public class AdminRepository
    {
        private readonly string _connStr;

        public AdminRepository()
        {
            _connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        }

        public bool Login(string email, string password)
        {
            // Hardcoded admin-login
            const string adminEmail = "admin";

            // password: azerty
            // const string adminPasswordHash = "8tgaJg3qihAN1ReYTlPFanUj2WlCqDS5zcJJvU6Meqk=";
            // pasword: admin
            const string adminPasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=";

            if (email == adminEmail)
            {
                return PasswordHasher.Verify(password, adminPasswordHash);
            }

            using SqlConnection conn = new SqlConnection(_connStr);
            conn.Open();
            string sql = "SELECT password FROM Companies WHERE email = @Email AND status = 'approved'";
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            var dbHashedPassword = cmd.ExecuteScalar() as string;
            return dbHashedPassword != null && PasswordHasher.Verify(password, dbHashedPassword);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CLBenchmark
{
    public static class NacecodeRepository
    {
        private static string _connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public static string HaalNaceOmschrijvingVoorBedrijf(int companyId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = @"
                SELECT nc.text
                FROM Companies c
                JOIN Nacecodes nc ON c.nacecode_code = nc.code
                WHERE c.id = @companyId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@companyId", companyId);

                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "Onbekende sector";
            }
        }
    }
}

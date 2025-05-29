using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CLBenchmark
{
    public class YearReportRepository
    {
        private readonly string _connStr;
        public YearReportRepository()
        {
            _connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        }

        public List<Yearreport> GetYearreportsForCompany(int companyId)
        {
            List<Yearreport> reports = new List<Yearreport>();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string sql = "SELECT id, year, fte, company_id FROM Yearreports WHERE company_id = @CompanyId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Yearreport yr = new Yearreport();
                            yr.Id = reader.GetInt32(0);
                            yr.Year = reader.GetInt32(1);
                            yr.Fte = reader.GetInt32(2);
                            yr.CompanyId = reader.GetInt32(3);
                            reports.Add(yr);
                        }
                    }
                }
            }
            return reports;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLBenchmark
{
    public class CostRepository
    {
        private readonly string _connStr;

        public CostRepository()
        {
            _connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        }

        public List<Cost> GetCostsForCompany(int companyId, int year, string costType)
        {
            List<Cost> costs = new List<Cost>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = @"
                SELECT c.value, ct.text AS CosttypeText, cat.text AS CategoryText
FROM Costs c
JOIN Costtypes ct ON ct.type = c.costtype_type
JOIN Categories cat ON cat.nr = c.category_nr
JOIN Yearreports y ON y.id = c.yearreport_id
WHERE y.company_id = @companyId AND y.year = @year AND ct.text = @costType";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyId", companyId);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@costType", costType);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cost cost = new Cost();
                            cost.Id = reader.GetInt32(0);
                            cost.Value = reader.GetDecimal(1);
                            cost.CostType = reader.GetString(4);     
                            cost.Category = reader.GetString(5);

                            costs.Add(cost);
                        }
                    }
                }
            }

            return costs;
        }

        public List<int> GetAvailableYears(int companyId)
        {
            List<int> years = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = "SELECT DISTINCT year FROM Yearreports WHERE company_id = @companyId ORDER BY year";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyId", companyId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            years.Add(reader.GetInt32(0));
                        }
                    }
                }
            }

            return years;
        }
        public List<string> GetAvailableCostTypes()
        {
            List<string> types = new List<string>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = "SELECT DISTINCT type FROM Costtypes ORDER BY type";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(reader.GetString(0));
                    }
                }
            }

            return types;
        }
        public List<Cost> GetFilteredCosts(int companyId, int year, string costType)
        {
            List<Cost> kosten = new List<Cost>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = @"
            SELECT c.id, c.value, c.costtype_type, c.category_nr, c.yearreport_id
            FROM Costs c
            INNER JOIN Yearreports y ON c.yearreport_id = y.id
            WHERE y.company_id = @companyId AND y.year = @year AND c.costtype_type = @costType";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyId", companyId);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@costType", costType);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cost cost = new Cost();
                            cost.Id = reader.GetInt32(0);
                            cost.Value = reader.GetDecimal(1);
                            cost.CostType = reader.GetString(2);
                            cost.Category = reader.GetInt32(3).ToString();
                            cost.YearreportId = reader.GetInt32(4);

                            kosten.Add(cost);
                        }
                    }
                }
            }

            return kosten;
        }
    }
}

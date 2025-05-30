using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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
        public List<Cost> GetFilteredCosts(int companyId, int? year, string costType)
        {
            List<Cost> kosten = new List<Cost>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = @"
                      SELECT c.id, c.value, ct.text AS CosttypeText, cat.nr AS CategoryNr, cat.text AS CategoryText, c.yearreport_id
            FROM Costs c
            JOIN Costtypes ct ON ct.type = c.costtype_type
            JOIN Categories cat ON cat.nr = c.category_nr
            JOIN Yearreports y ON y.id = c.yearreport_id
            WHERE y.company_id = @companyId";

                if (year.HasValue)
                {
                    query += " AND y.year = @year";
                }

                if (!string.IsNullOrEmpty(costType) && costType != "Alle types")
                {
                    query += " AND ct.type = @costType";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyId", companyId);
                    if (year.HasValue)
                        cmd.Parameters.AddWithValue("@year", year.Value);
                    if (!string.IsNullOrEmpty(costType) && costType != "Alle types")
                        cmd.Parameters.AddWithValue("@costType", costType);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cost cost = new Cost();

                            cost.Id = reader.GetInt32(0);                            // c.id
                            cost.Value = reader.GetDecimal(1);                      // c.value
                            cost.CostTypeText = reader.GetString(2);                // ct.text (CosttypeText)
                            cost.Category = reader.GetInt32(3).ToString();          // cat.nr (int -> string)
                            cost.CategoryText = reader.GetString(4);                // cat.text (CategoryText)
                            cost.YearreportId = reader.GetInt32(5);

                            kosten.Add(cost);
                        }
                    }
                }
                return kosten;
            }
        }

        public List<YearlyCostSummary> GetYearlyCostSummary(int companyId)
        {
            List<YearlyCostSummary> result = new List<YearlyCostSummary>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = @"
        SELECT y.year, ct.text AS CostType, SUM(c.value) AS Total
        FROM Costs c
        INNER JOIN Yearreports y ON y.id = c.yearreport_id
        INNER JOIN Costtypes ct ON ct.type = c.costtype_type
        WHERE y.company_id = @companyId
        GROUP BY y.year, ct.text
        ORDER BY y.year, ct.text";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyId", companyId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            YearlyCostSummary item = new YearlyCostSummary();
                            item.Year = reader.GetInt32(0);
                            item.CostType = reader.GetString(1);
                            item.Total = reader.GetDecimal(2);
                            result.Add(item);
                        }
                    }
                }
            }
            return result;
        }
        public decimal GetAverageOtherCompanyCost(int excludeCompanyId, int year, string type)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string query = @"
            SELECT AVG(c.value) 
            FROM Costs c
            JOIN Yearreports y ON y.id = c.yearreport_id
            WHERE y.year = @year 
              AND c.costtype_type = @type
              AND y.company_id != @excludeCompanyId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@excludeCompanyId", excludeCompanyId);

                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        public List<Company> GetAllOtherCompanies(int excludeCompanyId)
        {
            List<Company> bedrijven = new List<Company>();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string query = "SELECT id FROM Companies WHERE id != @excludeId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@excludeId", excludeCompanyId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bedrijven.Add(new Company
                            {
                                Id = reader.GetInt32(0)
                            });
                        }
                    }
                }
            }
            return bedrijven;
        }
        public decimal GetTotalCost(int companyId, int year, string costType)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = @"
            SELECT SUM(c.value)
            FROM Costs c
            JOIN Yearreports y ON y.id = c.yearreport_id
            WHERE y.company_id = @companyId AND y.year = @year AND c.costtype_type = @costType";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyId", companyId);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@costType", costType);

                    object result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        public List<CategoryCost> GetCategoryCosts(int companyId, int year, string costType)
        {
            List<CategoryCost> result = new List<CategoryCost>();

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string query = @"
            SELECT cat.text AS Category, SUM(c.value) AS Total
            FROM Costs c
            JOIN Categories cat ON cat.nr = c.category_nr
            JOIN Yearreports y ON y.id = c.yearreport_id
            WHERE y.company_id = @companyId AND y.year = @year AND c.costtype_type = @costType
            GROUP BY cat.text
            ORDER BY cat.text";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyId", companyId);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@costType", costType);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryCost item = new CategoryCost
                            {
                                Category = reader.GetString(0),
                                Total = reader.GetDecimal(1)
                            };
                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }
    }
}

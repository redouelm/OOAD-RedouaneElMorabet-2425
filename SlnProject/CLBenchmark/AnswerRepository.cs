using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLBenchmark
{
    public class AnswerRepository
    {
        private readonly string _connStr;

        public AnswerRepository()
        {
            _connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        }

        public List<(string QuestionText, string AnswerValue)> GetAnswersWithQuestions(int yearreportId)
        {
            List<(string, string)> results = new List<(string, string)>();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string sql = @"
            SELECT q.text, a.value
            FROM Answers a
            INNER JOIN Questions q ON a.question_id = q.id
            WHERE a.yearreport_id = @YearReportId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@YearReportId", yearreportId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string question = reader.GetString(0);
                            string answer = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            results.Add((question, answer));
                        }
                    }
                }
            }
            return results;
        }
    }
}

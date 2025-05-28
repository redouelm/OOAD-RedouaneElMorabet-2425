using CLBenchmark;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ConsolePasswordReset
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            List<string> lines = new List<string>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string selectQuery = "SELECT id, login FROM Companies WHERE password = 'hashedpassword'";
                SqlCommand selectCmd = new SqlCommand(selectQuery, conn);
                SqlDataReader reader = selectCmd.ExecuteReader();

                List<Tuple<int, string>> companies = new List<Tuple<int, string>>();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string login = reader.GetString(1);
                    companies.Add(new Tuple<int, string>(id, login));
                }
                reader.Close();

                foreach (Tuple<int, string> company in companies)
                {
                    int id = company.Item1;
                    string login = company.Item2;
                    string plainPassword = "test" + id.ToString();
                    string hashedPassword = PasswordHasher.Hash(plainPassword);

                    string updateQuery = "UPDATE Companies SET password = @Password WHERE id = @Id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@Password", hashedPassword);
                    updateCmd.Parameters.AddWithValue("@Id", id);
                    updateCmd.ExecuteNonQuery();

                    string logLine = id + "\t" + login + "\t" + plainPassword + "\t" + hashedPassword;
                    lines.Add(logLine);
                    Console.WriteLine(logLine);
                }
            }

            string downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloads, "UpdatedPasswords.txt");
            File.WriteAllLines(filePath, lines);

            Console.WriteLine("\n✅ Passwords opgeslagen in: " + filePath);
        }
    }
}

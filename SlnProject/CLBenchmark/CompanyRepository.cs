using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLBenchmark
{
    public class CompanyRepository
    {
        private readonly string _connStr;

        public CompanyRepository()
        {
            _connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        }

        public List<Company> GetAllCompanies()
        {
            var companies = new List<Company>();

            using SqlConnection conn = new SqlConnection(_connStr);
            conn.Open();

            string sql = "SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, status, language, logo FROM Companies";
            using SqlCommand cmd = new SqlCommand(sql, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                companies.Add(new Company
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Contact = reader.GetString(2),
                    Address = reader.GetString(3),
                    Zip = reader.GetString(4),
                    City = reader.GetString(5),
                    Country = reader.GetString(6),
                    Phone = reader.GetString(7),
                    Email = reader.GetString(8),
                    Btw = reader.GetString(9),
                    Login = reader.GetString(10),
                    Status = reader.GetString(11),
                    Language = reader.GetString(12),
                    Logo = reader.IsDBNull(13) ? null : (byte[])reader[13]
                });
            }

            return companies;
        }

        public void UpdateCompany(Company company)
        {
            using SqlConnection conn = new SqlConnection(_connStr);
            conn.Open();

            string sql = @"
        UPDATE Companies SET
            name = @name,
            contact = @contact,
            address = @address,
            zip = @zip,
            city = @city,
            country = @country,
            phone = @phone,
            email = @email,
            btw = @btw,
            login = @login,
            status = @status,
            language = @language,
            logo = @logo
        WHERE id = @id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", company.Id);
            cmd.Parameters.AddWithValue("@name", company.Name);
            cmd.Parameters.AddWithValue("@contact", company.Contact);
            cmd.Parameters.AddWithValue("@address", company.Address);
            cmd.Parameters.AddWithValue("@zip", company.Zip);
            cmd.Parameters.AddWithValue("@city", company.City);
            cmd.Parameters.AddWithValue("@country", company.Country);
            cmd.Parameters.AddWithValue("@phone", company.Phone);
            cmd.Parameters.AddWithValue("@email", company.Email);
            cmd.Parameters.AddWithValue("@btw", company.Btw);
            cmd.Parameters.AddWithValue("@login", company.Login);
            cmd.Parameters.AddWithValue("@status", company.Status);
            cmd.Parameters.AddWithValue("@language", company.Language);

            // Voor logo: als null, gebruik DBNull.Value
            if (company.Logo == null)
            {
                cmd.Parameters.Add(new SqlParameter("@logo", SqlDbType.Image)
                {
                    Value = DBNull.Value
                });
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@logo", SqlDbType.Image)
                {
                    Value = company.Logo
                });
            }

            cmd.ExecuteNonQuery();
        }
    }
}


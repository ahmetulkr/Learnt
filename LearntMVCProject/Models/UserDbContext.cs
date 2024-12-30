using System.Data.SqlClient;
using System.Configuration;
using System.Text.Json;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;

namespace LearntMVCProject.Models
{
    public class UserDbContext : DbContext

    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Course> Courses { get; set; }


      

        // SQL bağlantı dizesi (Connection String)
        private string connectionString = "Data Source=AHMET;Initial Catalog=UserDb;Integrated Security=True";

        // Kullanıcıyı veritabanına ekleyen metot
        public void AddUser(AppUser user)
        {
            // Kullanıcı nesnesini JSON'a çevir (Opsiyonel, direkt ekliyoruz)
            var jsonData = JsonSerializer.Serialize(user);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Users tablosuna veri ekleme sorgusu
                string query = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);

                // Bağlantıyı aç ve sorguyu çalıştır
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Kullanıcıyı veritabanından sorgulayıp getiren metot
        public AppUser GetUser(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Kullanıcıyı sorgulama (SELECT)
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                // Bağlantıyı aç
                con.Open();

                // Veri okuyucuyu çalıştır (ExecuteReader)
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())  // Kullanıcı bulundu
                    {
                        // Verileri AppUser nesnesine deserialize et
                        var user = new AppUser
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                        return user;
                    }
                }
            }
            return null;  // Kullanıcı bulunamazsa null döndür
        }
    }
}
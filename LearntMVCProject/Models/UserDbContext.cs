// using System.Data.SqlClient;
// using System.Configuration;
// using System.Text.Json;
// using System.Data.Entity;
// using System.Security.Cryptography.X509Certificates;

// namespace LearntMVCProject.Models
// {
//     public class UserDbContext : DbContext

//     
//         public DbSet<AppUser> Users { get; set; }
//         public DbSet<Course> Courses { get; set; }


      

//         // SQL bağlantı dizesi (Connection String)
//         private string connectionString = "Data Source=AHMET;Initial Catalog=UserDb;Integrated Security=True";

//         // Kullanıcıyı veritabanına ekleyen metot
//         public void AddUser(AppUser user)
//         {
//             // Kullanıcı nesnesini JSON'a çevir (Opsiyonel, direkt ekliyoruz)
//             var jsonData = JsonSerializer.Serialize(user);

//             using (SqlConnection con = new SqlConnection(connectionString))
//             {
//                 // Users tablosuna veri ekleme sorgusu
//                 string query = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)";
//                 SqlCommand cmd = new SqlCommand(query, con);
//                 cmd.Parameters.AddWithValue("@Username", user.Username);
//                 cmd.Parameters.AddWithValue("@Password", user.Password);
//                 cmd.Parameters.AddWithValue("@Email", user.Email);

//                 // Bağlantıyı aç ve sorguyu çalıştır
//                 con.Open();
//                 cmd.ExecuteNonQuery();
//             }
//         }

//         // Kullanıcıyı veritabanından sorgulayıp getiren metot
//         public AppUser GetUser(string username, string password)
//         {
//             using (SqlConnection con = new SqlConnection(connectionString))
//             {
//                 // Kullanıcıyı sorgulama (SELECT)
//                 string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
//                 SqlCommand cmd = new SqlCommand(query, con);
//                 cmd.Parameters.AddWithValue("@Username", username);
//                 cmd.Parameters.AddWithValue("@Password", password);

//                 // Bağlantıyı aç
//                 con.Open();

//                 // Veri okuyucuyu çalıştır (ExecuteReader)
//                 using (SqlDataReader reader = cmd.ExecuteReader())
//                 {
//                     if (reader.Read())  // Kullanıcı bulundu
//                     {
//                         // Verileri AppUser nesnesine deserialize et
//                         var user = new AppUser
//                         {
//                             Id = Convert.ToInt32(reader["Id"]),
//                             Username = reader["Username"].ToString(),
//                             Password = reader["Password"].ToString(),
//                             Email = reader["Email"].ToString()
//                         };
//                         return user;
//                     }
//                 }
//             }
//             return null;  // Kullanıcı bulunamazsa null döndür
//         }
//     }
// }
using Npgsql;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace LearntMVCProject.Models
{
    public class UserDbContext
    {
        private string connectionString = "Host=localhost;Port=5432;Database=UserDb;Username=postgres;Password=3302031";

        public void AddUser(AppUser user)
        {
            var jsonData = JsonSerializer.Serialize(user);

            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public AppUser GetUser(string username, string password)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new AppUser
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                    }
                }
            }
            return null;
        }
         public void AddOrUpdateProfile(UserProfile profile)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Profiles (UserId, FullName, Bio, ProfilePictureUrl)
                    VALUES (@UserId, @FullName, @Bio, @ProfilePictureUrl)
                    ON CONFLICT (UserId) 
                    DO UPDATE SET 
                    FullName = @FullName,
                    Bio = @Bio,
                    ProfilePictureUrl = @ProfilePictureUrl";
                
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", profile.UserId);
                cmd.Parameters.AddWithValue("@FullName", profile.FullName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Bio", profile.Bio ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProfilePictureUrl", profile.ProfilePictureUrl ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public UserProfile GetProfile(int userId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM Profiles WHERE UserId = @UserId";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            FullName = reader["FullName"].ToString(),
                            Bio = reader["Bio"].ToString(),
                            ProfilePictureUrl = reader["ProfilePictureUrl"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}

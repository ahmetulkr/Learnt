using Npgsql;
using System.Text.Json;

namespace LearntMVCProject.Models
{
    public class UserDbContext
    {
        private string connectionString = "Host=localhost;Port=5432;Database=UserDb;Username=postgres;Password=3302031";

        // Kullanıcı ekleme
        public void AddUser(AppUser user)
        {
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

        // Kullanıcıyı username ve password ile getir
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

        // Profil ekleme veya güncelleme
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
        
        // Kullanıcının profilini getir
        public UserProfile GetProfile(int userId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT UserId, FullName, Bio, ProfilePictureUrl FROM Profiles WHERE UserId = @UserId";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserProfile
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            FullName = reader["FullName"].ToString(),
                            Bio = reader["Bio"]?.ToString() ?? "",
                            ProfilePictureUrl = reader["ProfilePictureUrl"]?.ToString() ?? "/images/default-avatar.png"
                        };
                    }
                }
            }
            return null;
        }

        // Varsayılan profil oluştur
        public void CreateDefaultProfile(int userId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Profiles (UserId, FullName, Bio, ProfilePictureUrl) 
                    VALUES (@UserId, @FullName, @Bio, @ProfilePictureUrl)
                    ON CONFLICT (UserId) DO NOTHING;";

                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@FullName", "Yeni Kullanıcı");
                cmd.Parameters.AddWithValue("@Bio", "");
                cmd.Parameters.AddWithValue("@ProfilePictureUrl", "/images/default-avatar.png");

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

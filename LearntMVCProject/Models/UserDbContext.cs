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
                            Email = reader["Email"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        // User Düzenleme Yeri Profil ekleme veya güncelleme
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
        
        // Admin kullanıcıyı getirmek için
        public Admin GetAdmin(string adminUser, string adminPassword)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM Admin WHERE AdminUser = @AdminUser AND AdminPassword = @AdminPassword";
                var cmd = new NpgsqlCommand(query, con);

                cmd.Parameters.AddWithValue("@AdminUser", adminUser);
                cmd.Parameters.AddWithValue("@AdminPassword", adminPassword);

                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Admin
                        {
                            AdminId = reader.GetInt32(reader.GetOrdinal("AdminId")),
                            AdminUser = reader.GetString(reader.GetOrdinal("AdminUser")),
                            AdminPassword = reader.GetString(reader.GetOrdinal("AdminPassword")),
                        };
                    }
                }
            }
            return null;
        }
        
        // Kurs ekleme
        public int AddCourse(Course course)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Courses (Name, Description, ImageUrl, CreatedDate, IsActive, AdminId) 
                    VALUES (@Name, @Description, @ImageUrl, @CreatedDate, @IsActive, @AdminId)
                    RETURNING Id";
                
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", course.Name);
                cmd.Parameters.AddWithValue("@Description", course.Description);
                cmd.Parameters.AddWithValue("@ImageUrl", course.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedDate", course.CreatedDate);
                cmd.Parameters.AddWithValue("@IsActive", course.IsActive);
                cmd.Parameters.AddWithValue("@AdminId", course.AdminId);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        
        // Kurs güncelleme
        public void UpdateCourse(Course course)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = @"
                    UPDATE Courses 
                    SET Name = @Name, 
                        Description = @Description, 
                        ImageUrl = @ImageUrl, 
                        IsActive = @IsActive 
                    WHERE Id = @Id";
                
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", course.Id);
                cmd.Parameters.AddWithValue("@Name", course.Name);
                cmd.Parameters.AddWithValue("@Description", course.Description);
                cmd.Parameters.AddWithValue("@ImageUrl", course.ImageUrl ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", course.IsActive);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Kurs silme
        public void DeleteCourse(int courseId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                // Önce ilişkili videoları sil
                string deleteVideosQuery = "DELETE FROM VideoLessons WHERE CourseId = @CourseId";
                var deleteVideosCmd = new NpgsqlCommand(deleteVideosQuery, con);
                deleteVideosCmd.Parameters.AddWithValue("@CourseId", courseId);
                
                // Sonra kursu sil
                string deleteCourseQuery = "DELETE FROM Courses WHERE Id = @Id";
                var deleteCourseCmd = new NpgsqlCommand(deleteCourseQuery, con);
                deleteCourseCmd.Parameters.AddWithValue("@Id", courseId);
                
                con.Open();
                deleteVideosCmd.ExecuteNonQuery();
                deleteCourseCmd.ExecuteNonQuery();
            }
        }
        
        // Tüm kursları getir
        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM Courses ORDER BY CreatedDate DESC";
                var cmd = new NpgsqlCommand(query, con);
                
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            ImageUrl = reader["ImageUrl"] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            AdminId = reader.GetInt32(reader.GetOrdinal("AdminId"))
                        });
                    }
                }
            }
            
            return courses;
        }
        
        // Belirli bir kursu getir
        public Course GetCourseById(int courseId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM Courses WHERE Id = @Id";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", courseId);
                
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Course
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            ImageUrl = reader["ImageUrl"] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            AdminId = reader.GetInt32(reader.GetOrdinal("AdminId"))
                        };
                    }
                }
            }
            
            return null;
        }
        
        // Video dersi ekleme
        public int AddVideoLesson(VideoLesson video)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO VideoLessons (Title, Description, VideoUrl, OrderIndex, UploadDate, IsActive, CourseId) 
                    VALUES (@Title, @Description, @VideoUrl, @OrderIndex, @UploadDate, @IsActive, @CourseId)
                    RETURNING Id";
                
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Title", video.Title);
                cmd.Parameters.AddWithValue("@Description", video.Description);
                cmd.Parameters.AddWithValue("@VideoUrl", video.VideoUrl);
                cmd.Parameters.AddWithValue("@OrderIndex", video.OrderIndex);
                cmd.Parameters.AddWithValue("@UploadDate", video.UploadDate);
                cmd.Parameters.AddWithValue("@IsActive", video.IsActive);
                cmd.Parameters.AddWithValue("@CourseId", video.CourseId);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        
        // Video dersi güncelleme
        public void UpdateVideoLesson(VideoLesson video)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = @"
                    UPDATE VideoLessons 
                    SET Title = @Title, 
                        Description = @Description, 
                        VideoUrl = @VideoUrl, 
                        OrderIndex = @OrderIndex, 
                        IsActive = @IsActive 
                    WHERE Id = @Id";
                
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", video.Id);
                cmd.Parameters.AddWithValue("@Title", video.Title);
                cmd.Parameters.AddWithValue("@Description", video.Description);
                cmd.Parameters.AddWithValue("@VideoUrl", video.VideoUrl);
                cmd.Parameters.AddWithValue("@OrderIndex", video.OrderIndex);
                cmd.Parameters.AddWithValue("@IsActive", video.IsActive);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Video dersi silme
        public void DeleteVideoLesson(int videoId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "DELETE FROM VideoLessons WHERE Id = @Id";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", videoId);
                
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Bir kursa ait tüm video dersleri getir
        public List<VideoLesson> GetVideoLessonsByCourseId(int courseId)
        {
            List<VideoLesson> videos = new List<VideoLesson>();
            
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM VideoLessons WHERE CourseId = @CourseId ORDER BY OrderIndex";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        videos.Add(new VideoLesson
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            VideoUrl = reader.GetString(reader.GetOrdinal("VideoUrl")),
                            OrderIndex = reader.GetInt32(reader.GetOrdinal("OrderIndex")),
                            UploadDate = reader.GetDateTime(reader.GetOrdinal("UploadDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            CourseId = reader.GetInt32(reader.GetOrdinal("CourseId"))
                        });
                    }
                }
            }
            
            return videos;
        }
        
        // Belirli bir video dersi getir
        public VideoLesson GetVideoLessonById(int videoId)
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM VideoLessons WHERE Id = @Id";
                var cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", videoId);
                
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new VideoLesson
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            VideoUrl = reader.GetString(reader.GetOrdinal("VideoUrl")),
                            OrderIndex = reader.GetInt32(reader.GetOrdinal("OrderIndex")),
                            UploadDate = reader.GetDateTime(reader.GetOrdinal("UploadDate")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            CourseId = reader.GetInt32(reader.GetOrdinal("CourseId"))
                        };
                    }
                }
            }
            
            return null;
        }
    }
}

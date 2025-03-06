using Npgsql;
using System;

namespace LearntMVCProject.Models
{
    public class DatabaseInitializer
    {
        private readonly string connectionString;

        public DatabaseInitializer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Initialize()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Users tablosu (Role alanı ile birlikte)
                    using (var command = new NpgsqlCommand(
                        @"CREATE TABLE IF NOT EXISTS Users (
                            Id SERIAL PRIMARY KEY, 
                            Username VARCHAR(100) NOT NULL, 
                            Password VARCHAR(100) NOT NULL,
                            Email VARCHAR(255) NOT NULL,
                            Role VARCHAR(50) DEFAULT 'user'
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    // Courses tablosu
                    using (var command = new NpgsqlCommand(
                        @"CREATE TABLE IF NOT EXISTS Courses (
                            Id SERIAL PRIMARY KEY, 
                            Name VARCHAR(255) NOT NULL, 
                            Description TEXT NOT NULL, 
                            ImageUrl TEXT, 
                            CreatedDate TIMESTAMP NOT NULL, 
                            IsActive BOOLEAN NOT NULL, 
                            AdminId INTEGER NOT NULL,
                            FOREIGN KEY (AdminId) REFERENCES Users(Id)
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    // VideoLessons tablosu
                    using (var command = new NpgsqlCommand(
                        @"CREATE TABLE IF NOT EXISTS VideoLessons (
                            Id SERIAL PRIMARY KEY, 
                            Title VARCHAR(255) NOT NULL, 
                            Description TEXT NOT NULL, 
                            VideoUrl TEXT NOT NULL, 
                            OrderIndex INTEGER NOT NULL, 
                            UploadDate TIMESTAMP NOT NULL, 
                            IsActive BOOLEAN NOT NULL, 
                            CourseId INTEGER NOT NULL,
                            FOREIGN KEY (CourseId) REFERENCES Courses(Id) ON DELETE CASCADE
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    // Profiles tablosu
                    using (var command = new NpgsqlCommand(
                        @"CREATE TABLE IF NOT EXISTS Profiles (
                            UserId INTEGER PRIMARY KEY,
                            FullName VARCHAR(255),
                            Bio TEXT,
                            ProfilePictureUrl TEXT,
                            FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    // Varsayılan admin kullanıcısı oluştur (eğer yoksa)
                    using (var command = new NpgsqlCommand(
                        @"INSERT INTO Users (Username, Password, Email, Role) 
                          SELECT 'admin', 'admin123', 'admin@example.com', 'admin' 
                          WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    Console.WriteLine("Veritabanı tabloları başarıyla oluşturuldu.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanı tabloları oluşturulurken hata oluştu: {ex.Message}");
                throw;
            }
        }
    }
} 
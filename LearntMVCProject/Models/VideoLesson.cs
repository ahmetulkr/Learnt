using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearntMVCProject.Models
{
    public class VideoLesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int OrderIndex { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Foreign key for Course
        public int CourseId { get; set; }
        
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        
        public VideoLesson()
        {
            UploadDate = DateTime.Now;
        }
    }
} 
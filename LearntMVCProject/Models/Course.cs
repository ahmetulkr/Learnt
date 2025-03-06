namespace LearntMVCProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int AdminId { get; set; }
        
        // Navigation property for videos
        public virtual ICollection<VideoLesson> VideoLessons { get; set; }
        
        public Course()
        {
            VideoLessons = new List<VideoLesson>();
            CreatedDate = DateTime.Now;
        }
    }
}

namespace LearntMVCProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public int TeacherId { get; set; }
        public AppUser Teacher { get; set; }

    }
}

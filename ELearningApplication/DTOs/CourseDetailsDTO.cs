namespace ELearningApplication.DTOs
{
    public class CourseDetailsDTO
    {
        public string CourseName { get; set; }
        public string CourseType { get; set; }
        public string Image { get; set; }
        public string LessonDescription { get; set; }
        public List<ClipGetDTO> Clips { get; set; }
    }

    public class ClipGetDTO
    {
        public string ClipName { get; set; }
        public string ClipUrl { get; set; }
    }
}

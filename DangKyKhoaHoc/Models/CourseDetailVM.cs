namespace DangKyKhoaHoc.Models
{
    public class CourseDetailVM
    {
        public Course Course { get; set; }
        public List<FeedBack> Feedbacks { get; set; }
        public List<CourseMaterial> Materials { get; set; }

    }
}

using ELearningApplication.DTOs;
using ELearningApplication.StoredProcedure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ELearningApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ELearningApplicationContext _context;

        public CourseController(ELearningApplicationContext context)
        {
            _context = context;
        }

        //GET Courses 
        [HttpGet]
        public async Task<ActionResult<List<CoursesGetDTO>>> GetCourses()
        {
            /* return Ok(await _context.Courses.ToListAsync());*/
            var courses = await _context.Courses.ToListAsync();
            var coursesDTO = courses.Select(c => new CoursesGetDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                CourseType = c.CourseType,
                Image = c.Image,
            }).ToList();

            return Ok(coursesDTO);
        }
        [HttpGet("{courseId}")]
        public async Task<ActionResult<List<CourseDetailsDTO>>> GetCourseDetails(int courseId)
        {
            var courseDetails = await _context.GetCourseDetails(courseId);

            var groupedCourseDetails = courseDetails.GroupBy(cd => new
            {
                cd.CourseName,
                cd.CourseType,
                cd.Image,
                cd.LessonDescription
            }).Select(group => new CourseDetailsDTO
            {
                CourseName = group.Key.CourseName,
                CourseType = group.Key.CourseType,
                Image = group.Key.Image,
                LessonDescription = group.Key.LessonDescription,
                Clips = group.Select(cd => new ClipGetDTO
                {
                    ClipName = cd.ClipName,
                    ClipUrl = cd.ClipUrl
                }).ToList()
            }).ToList();

            return Ok(groupedCourseDetails);
        }
        private const string MY_API_KEY = "AIzaSyAVfztjGa7togFfy - c4gjGyBStTJp3PP9M";
        private string GetVideoIdFromUrl(string url)
        {

            // Lấy vị trí của tham số "v=" trong đường dẫn
            int parameterIndex = url.IndexOf("v=");

            // Lấy phần tử sau tham số "v=" là ID video
            string videoId = url.Substring(parameterIndex + 2);

            int ampersandIndex = videoId.IndexOf("&");
            if (ampersandIndex != -1)
            {
                videoId = videoId.Substring(0, ampersandIndex);
            }



            return videoId;
        }

        [HttpPost("Clip")]
        public async Task<IActionResult> AddClip([FromBody] CourseAddClip clip)
        {
            try
            {
                // Gọi stored procedure để thêm clip
                await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE dbo.AddClip {clip.ClipName}, {clip.ClipUrl}, {clip.LessonId}");
                string videoId = GetVideoIdFromUrl(clip.ClipUrl);
                // Gửi yêu cầu GET đến API của YouTube để lấy thông tin video
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"https://www.googleapis.com/youtube/v3/videos?part=contentDetails&id={videoId}&key={MY_API_KEY}");
                    string json = await response.Content.ReadAsStringAsync();

                    // Phân tích kết quả JSON để lấy thời lượng video
                    JObject parsedJson = JObject.Parse(json);
                    JToken durationToken = parsedJson["items"]?[0]?["contentDetails"]?["duration"];
                    string duration = durationToken?.Value<string>();

                    // Xử lý thông tin thời lượng của video tại đây
                    // Ví dụ: Chuyển đổi thời lượng từ định dạng ISO 8601 sang đơn vị phút

                    return Ok($"Clip added successfully., id clip: {videoId}, duration: {duration}");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}

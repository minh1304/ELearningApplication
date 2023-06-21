using ELearningApplication.DTOs;
using Microsoft.AspNetCore.Mvc;

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
    }
}

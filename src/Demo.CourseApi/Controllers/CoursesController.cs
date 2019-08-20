using Demo.CourseApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Demo.CourseApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : Controller
    {
        static Dictionary<int, CourseModel> courses = new Dictionary<int, CourseModel>() {
            {1,  new  CourseModel(){ Id = 1, Name = "C# Programing for Beginers", Student = 10} },
            {2,  new CourseModel(){Id = 2, Name = "Java Programing for Beginers", Student = 50} }
        };
        [HttpGet]
        public IEnumerable<object> GetAll()
        {
            return courses.Values;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public object GetById(int id)
        {
            return courses[id];
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddCourse([FromBody]CourseModel course)
        {
            var max = courses.Keys.Max();
            course.Id = ++max;
            courses[max] = course;
            return Created($"/api/courses/{max}", course);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCourse(int id, [FromBody]CourseModel course)
        {
            course.Id = id;
            courses[id] = course;
            return Accepted($"/api/courses/{id}", course);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(int id)
        {
            var exists = courses.ContainsKey(id);
            if (exists)
            {
                courses.Remove(id);
            }
            return Ok(exists);
        }

        [HttpGet("info")]
        [AllowAnonymous]
        public IActionResult GetUserInfomation()
        {
            var claims = User.Claims.Select(x => new { Name = x.Type, Value = x.Value });
            return Ok(claims);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Telhai.CS.ServerAPI.Models;
using Telhai.CS.ServerAPI.Repos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelHai.CS.SereverAPI.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentsRepository repo;
        public StudentsController()
        {
            repo = new StudentsRepository();
            Student s1 = new Student("Donald",60,"cs");
            Student s2 = new Student("Mickey", 34, "sw");
            repo.AddStudent(s1);
            repo.AddStudent(s2);
        }
        // GET: api/<StudentsController>
       [HttpGet]
        public IEnumerable<Student> Get()
        {
            //return getStudentsDB();
            return repo.Students;
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public Student Get(string id)
        {
            Student? stu =  this.repo.Students.Where(s => s.Id == id).FirstOrDefault();
            return stu;
        }

        // POST api/<StudentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

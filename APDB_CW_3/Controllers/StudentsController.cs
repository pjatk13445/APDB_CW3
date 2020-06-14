using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APDB_CW_3.DAL;
using APDB_CW_3.Models;
using Microsoft.AspNetCore.Mvc;


namespace APDB_CW_3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            List<Student> students = new List<Student>();
            using (var con =
                new SqlConnection("Data Source=127.0.0.1,1433;Database=master;User Id=sa;Password=password1337;"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM dbo.Student";
                con.Open();
                var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    students.Add(new Student(
                        reader["FirstName"].ToString(),
                        reader["LastName"].ToString(),
                        reader["IndexNumber"].ToString(),
                        int.Parse(reader["IdEnrollment"].ToString()),
                        DateTime.Parse(reader["BirthDate"].ToString())
                    ));
                }
            }

            return Ok(students);
        }

        [HttpGet("{IndexNumber}/enrollments")]
        public IActionResult GetEnrollments(string IndexNumber)
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            using (var con =
                new SqlConnection("Data Source=127.0.0.1,1433;Database=master;User Id=sa;Password=password1337;"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText =
                    "SELECT E.* FROM dbo.Enrollment E JOIN dbo.Student S ON S.IdEnrollment = E.IdEnrollment WHERE S.IndexNumber = @IndexNumber";
                com.Parameters.AddWithValue("IndexNumber", IndexNumber);
                con.Open();
                var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    enrollments.Add(new Enrollment(
                        int.Parse(reader["IdEnrollment"].ToString()),
                        int.Parse(reader["Semester"].ToString()),
                        int.Parse(reader["IdStudy"].ToString()),
                        DateTime.Parse(reader["StartDate"].ToString())
                    ));
                }
            }

            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Marek");
            }
            else if (id == 2)
            {
                return Ok("Malewski");
            }

            return NotFound("Nie znaleziono");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(Student student)
        {
            return Ok("Aktualizacja zakończona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            if (id == 1 || id == 2)
            {
                return Ok("Usuwanie zakończone");
            }

            return NotFound("Nie znaleziono");
        }
    }
}
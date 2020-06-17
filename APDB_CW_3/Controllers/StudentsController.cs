using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using APDB_CW_3.DbModels;
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
            var db = new masterContext();
            return Ok(db.Student.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(String IndexNumber)
        {
            var context = new masterContext();
            return Ok(context.Student.Find(IndexNumber));
        }

        [HttpPost]
        public IActionResult CreateStudent(CreateStudentPayload payload)
        {
            var context = new masterContext();
            var student = new Student
            {
                BirthDate = payload.BirthDate,
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                Role = payload.Role,
                IndexNumber = payload.IndexNumber,
            };
            context.Student.Add(student);
            context.SaveChanges();

            return Ok(student);
        }

        [HttpPut("{indexNumber}")]
        public IActionResult PutStudent(PutStudentPayload payload, string indexNumber)
        {
            var context = new masterContext();
            var studentInDb = context.Student.Find(indexNumber);
            if (studentInDb == null)
            {
                return NotFound();
            }

            studentInDb.FirstName = payload.FirstName ?? studentInDb.FirstName;
            studentInDb.LastName = payload.LastName ?? studentInDb.LastName;
            studentInDb.BirthDate = payload.BirthDate ?? studentInDb.BirthDate;
            context.SaveChanges();

            return Ok("Aktualizacja zakończona");
        }

        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            var context = new masterContext();
            var studentInDb = context.Student.Find(indexNumber);

            context.Student.Remove(studentInDb);
            context.SaveChanges();

            return NoContent();
        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using APDB_CW_3.DbModels;
using APDB_CW_3.Models;
using APDB_CW_3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APDB_CW_3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    // [Authorize(Roles = "employee")]
    public class EnrollmentsController : Controller
    {
        private SqlServerDbService db;

        public EnrollmentsController(SqlServerDbService db)
        {
            this.db = db;
        }

        [HttpPost]
        public IActionResult EntrollStudent(EnrollStudentPayload payload)
        {
            if (
                String.IsNullOrEmpty(payload.IndexNumber) ||
                String.IsNullOrEmpty(payload.FirstName) ||
                String.IsNullOrEmpty(payload.LastName) ||
                String.IsNullOrEmpty(payload.BirthDate) ||
                String.IsNullOrEmpty(payload.Studies)
            )
            {
                return BadRequest("Wypełnij wszystkie dane");
            }

            var context = new masterContext();
            var study = context.Studies.Single(s => s.Name.Equals(payload.Studies));
            if (study == null)
            {
                return BadRequest("Nie ma takich studiów");
            }

            var enrollment = context.Enrollment
                .Where(e => e.Semester.Equals(1))
                .SingleOrDefault(e => e.IdStudy.Equals(study.IdStudy));
            if (enrollment == null)
            {
                enrollment = new Enrollment
                {
                    Semester = 1,
                    IdStudy = study.IdStudy,
                    StartDate = DateTime.Now
                };
                context.Enrollment.Add(enrollment);
                context.SaveChanges();
            }

            var student = new Student
            {
                IndexNumber = payload.IndexNumber,
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                BirthDate = DateTime.Parse(payload.BirthDate),
                Role = "student",
                IdEnrollment = enrollment.IdEnrollment
            };
            context.Student.Add(student);
            context.SaveChanges();

            return Ok("Enrolled");
        }

        [HttpPost]
        [Route("promotions")]
        public IActionResult promote(PromoteStudentsPayload payload)
        {
            var context = new masterContext();
            var study = context.Studies.Single(s => s.Name.Equals(payload.Studies));
            if (study == null)
            {
                return BadRequest("Nie ma takich studiów");
            }

            var oldEnrolment = context.Enrollment
                .Where(e => e.Semester.Equals(payload.Semester))
                .SingleOrDefault(e => e.IdStudy.Equals(study.IdStudy));
            if (oldEnrolment == null)
            {
                return BadRequest("Nie znaleziono zapisów na podane studia");
            }

            var newEnrolment = context.Enrollment
                .Where(e => e.Semester.Equals(payload.Semester + 1))
                .SingleOrDefault(e => e.IdStudy.Equals(study.IdStudy));

            if (newEnrolment == null)
            {
                var newEnrollment = new Enrollment()
                {
                    Semester = oldEnrolment.Semester + 1,
                    IdStudy = study.IdStudy,
                    StartDate = DateTime.Now
                };
                context.Enrollment.Add(newEnrolment);
                context.SaveChanges();
            }

            var studentsToPromote = context.Student.Where(s => s.IdEnrollment.Equals(oldEnrolment.IdEnrollment))
                .AsEnumerable();
            foreach (var student in studentsToPromote)
            {
                student.IdEnrollment = newEnrolment.IdEnrollment;
            }

            context.SaveChanges();

            return Ok("All students promoted :)");
        }
    }
}
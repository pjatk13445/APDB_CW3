using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using APDB_CW_3.Models;
using APDB_CW_3.Services;
using Microsoft.AspNetCore.Mvc;

namespace APDB_CW_3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : Controller
    {
        private IStudentsDbService db;

        public EnrollmentsController(IStudentsDbService db)
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

            var idStudy = db.GetIdStudyByName(payload.Studies);
            if (idStudy == null)
            {
                return BadRequest("Nie ma takich studiów");
            }

            if (db.EnrollStudent(
                (int) idStudy,
                payload.IndexNumber,
                payload.FirstName,
                payload.LastName,
                payload.BirthDate
            ))
            {
                return Ok();
            }

            return Problem("Wystąpił błąd bazy danych");
        }

        [HttpPost]
        [Route("promotions")]
        public IActionResult promote(PromoteStudentsPayload payload)
        {
            using (var con =
                new SqlConnection("Data Source=127.0.0.1,1433;Database=master;User Id=sa;Password=password1337;"))
            using (var com = new SqlCommand())
            {
                var idStudy = db.GetIdStudyByName(payload.Studies);
                if (idStudy == null)
                {
                    return BadRequest("Nie ma takich studiów.");
                }

                var idEnrollment = db.GetIdEnrollment((int) idStudy, payload.Semester);
                if (idEnrollment == null)
                {
                    return BadRequest("Brak zapisów na podane studia");
                }

                if (db.PromoteStudents(payload.Studies, payload.Semester))
                {
                    return Ok("Wszyscy studenci otrzymali promocję :)");
                }

                return Problem("Wystąpikł błąd bazy danych");
            }
        }
    }
}
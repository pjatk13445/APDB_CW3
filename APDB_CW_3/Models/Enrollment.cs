using System;

namespace APDB_CW_3.Models
{
    public class Enrollment
    {
        public Enrollment(int idEnrollment, int semester, int idStudy, DateTime startDate)
        {
            IdEnrollment = idEnrollment;
            Semester = semester;
            IdStudy = idStudy;
            StartDate = startDate;
        }

        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }
    }
}
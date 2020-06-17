using System;

namespace APDB_CW_3.Models
{
    public class CreateStudentPayload
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
        public string Role { get; set; }
    }
}
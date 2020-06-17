using System;

namespace APDB_CW_3.Models
{
    public class PutStudentPayload
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
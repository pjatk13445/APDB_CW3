using System;

namespace APDB_CW_3.Models
{
    public class Student
    {
        public Student(string firstName, string lastName, string indexNumber, int idEnrollment, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            IndexNumber = indexNumber;
            IdEnrollment = idEnrollment;
            BirthDate = birthDate;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public int IdEnrollment { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
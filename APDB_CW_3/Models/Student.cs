namespace APDB_CW_3.Models
{
    public class Student
    {
        public Student(int idStudent, string firstName, string lastName)
        {
            IdStudent = idStudent;
            FirstName = firstName;
            LastName = lastName;
        }

        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
    }
}
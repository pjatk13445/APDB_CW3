using System.Collections.Generic;
using APDB_CW_3.Models;

namespace APDB_CW_3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students = new List<Student>
        {
            new Student(1, "Jan", "Kowalski"),
            new Student(2, "Janusz", "Kowalski"),
            new Student(3, "Mariusz", "Kowalski"),
        };

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using APDB_CW_3.Models;

namespace APDB_CW_3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}
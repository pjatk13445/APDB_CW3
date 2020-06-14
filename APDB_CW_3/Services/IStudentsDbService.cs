using APDB_CW_3.Models;

namespace APDB_CW_3.Services
{
    public interface IStudentsDbService
    {
        public int? GetIdStudyByName(string Studies);

        public bool EnrollStudent(
            int IdStudy,
            string IndexNumber,
            string FirstName,
            string LastName,
            string BirthDate
        );

        public bool PromoteStudents(string Studies, int Semester);

        public int? GetIdEnrollment(int IdStudy, int Semester);

        public bool StudentExists(string IndexNumber);

        public StudentSecurityData GetStudentSecurityData(string IndexNumber);

        public void UpdatePassword(string IndexNumber, string Salt, string PasswordHash);

        public void UpdateRefreshToken(string IndexNumber, string RefreshToken);
    }
}
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
    }
}
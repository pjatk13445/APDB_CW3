using System;
using System.Data;
using System.Data.SqlClient;
using APDB_CW_3.Models;

namespace APDB_CW_3.Services
{
    public class SqlServerDbService : IStudentsDbService
    {
        public int? GetIdStudyByName(string Studies)
        {
            using (var con =
                new SqlConnection("Data Source=127.0.0.1,1433;Database=master;User Id=sa;Password=password1337;"))
            using (var com = new SqlCommand())
            {
                com.CommandText = "SELECT IdStudy FROM dbo.Studies WHERE Name = @studies";
                com.Parameters.AddWithValue("studies", Studies);
                com.Connection = con;
                con.Open();
                var result = com.ExecuteScalar();
                if (result != null)
                {
                    return int.Parse(result.ToString());
                }
            }

            return null;
        }

        public bool EnrollStudent(
            int IdStudy,
            string IndexNumber,
            string FirstName,
            string LastName,
            string BirthDate
        )
        {
            using (var con =
                new SqlConnection("Data Source=127.0.0.1,1433;Database=master;User Id=sa;Password=password1337;"))
            using (var com = new SqlCommand())
            {
                con.Open();
                var transaction = con.BeginTransaction();
                try
                {
                    using (var com2 = new SqlCommand())
                    {
                        com2.Connection = con;
                        com2.Transaction = transaction;
                        com2.CommandText =
                            "SELECT IdEnrollment FROM dbo.Enrollment WHERE IdStudy = @idStudy AND Semester = 1 ORDER BY StartDate DESC ";
                        com2.Parameters.AddWithValue("idStudy", IdStudy);
                        object EnrollmentRersult = com2.ExecuteScalar();
                        int IdEnrollment;
                        if (EnrollmentRersult != null)
                        {
                            IdEnrollment = int.Parse(EnrollmentRersult.ToString());
                        }
                        else
                        {
                            using (var com3 = new SqlCommand())
                            {
                                com3.Connection = con;
                                com3.Transaction = transaction;
                                com3.CommandText =
                                    "INSERT INTO dbo.Enrollment (IdEnrollment, Semester, IdStudy, StartDate) output INSERTED.IdEnrollment VALUES ((SELECT COALESCE(MAX(IdEnrollment), 0) + 1 FROM dbo.Enrollment), 1, @idStudy, CURRENT_TIMESTAMP)";
                                com3.Parameters.AddWithValue("idStudy", IdStudy);
                                IdEnrollment = int.Parse(com3.ExecuteScalar().ToString());
                            }
                        }

                        using (var con3 = new SqlCommand())
                        {
                            con3.Connection = con;
                            con3.Transaction = transaction;
                            con3.CommandText =
                                "INSERT INTO dbo.Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)";
                            con3.Parameters.AddWithValue("indexNumber", IndexNumber);
                            con3.Parameters.AddWithValue("firstName", FirstName);
                            con3.Parameters.AddWithValue("lastName", LastName);
                            con3.Parameters.AddWithValue("birthDate", DateTime.Parse(
                                BirthDate
                            ));
                            con3.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                            con3.ExecuteNonQuery();
                            transaction.Commit();
                            var enrolment = new Enrollment(IdEnrollment, 1, IdStudy, DateTime.Now);
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool PromoteStudents(string Studies, int Semester)
        {
            using (var con =
                new SqlConnection("Data Source=127.0.0.1,1433;Database=master;User Id=sa;Password=password1337;"))
            using (var com3 = new SqlCommand())
            {
                try
                {
                    con.Open();
                    com3.Transaction = con.BeginTransaction();
                    com3.Connection = con;
                    com3.CommandType = CommandType.StoredProcedure;
                    com3.CommandText = "Promote";
                    com3.Parameters.AddWithValue("Studies", Studies);
                    com3.Parameters.AddWithValue("Semester", Semester);
                    com3.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    com3.Transaction.Rollback();
                    return false;
                }

                return true;
            }

            return false;
        }

        public int? GetIdEnrollment(int IdStudy, int Semester)
        {
            using (var con =
                new SqlConnection("Data Source=127.0.0.1,1433;Database=master;User Id=sa;Password=password1337;"))
            using (var com2 = new SqlCommand())
            {
                con.Open();
                com2.CommandText =
                    "SELECT IdEnrollment FROM dbo.Enrollment WHERE IdStudy = @idStudy AND Semester = @semester";
                com2.Parameters.AddWithValue("idStudy", IdStudy);
                com2.Parameters.AddWithValue("semester", Semester);
                com2.Connection = con;
                object enrollmentRersult = com2.ExecuteScalar();
                if (enrollmentRersult == null)
                {
                    return null;
                }

                return int.Parse(enrollmentRersult.ToString());
            }
        }
    }
}
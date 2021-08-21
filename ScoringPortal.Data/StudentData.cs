using Microsoft.Data.SqlClient;
using ScoringPortal.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoringPortal.Data
{
    public class StudentData : AccountData
    {
        public StudentData(string masterConnectionString, string myConnectionString) : base(masterConnectionString, myConnectionString)
        {}

        public async Task<IEnumerable<Score>> GetScoresBySubject(int studentID, int subjectID)
        {
            string sql = @"select sc.ID, sc.StudentID, s.FullName StudentName
                                , sc.TeacherID, t.FullName TeacherName
                                , sc.SubjectID, sb.[Name] SubjectName, sc.Score
                           from dbo.Scores sc
                               join dbo.Accounts s on s.AccountType = 2 and s.ID = sc.StudentID
                               join dbo.Accounts t on t.AccountType = 1 and t.ID = sc.TeacherID
                               join dbo.Subjects sb on sb.ID = sc.SubjectID
                           where sc.StudentID = @studentID
                             and sc.SubjectID = @subjectID";
            List<Score> result = new List<Score>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.Parameters.Add(new SqlParameter("@studentID", studentID));
                        cmd.Parameters.Add(new SqlParameter("@subjectID", subjectID));
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                result.Add(new Score
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                                    TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                    TeacherName = reader.GetString(reader.GetOrdinal("TeacherName")),
                                    SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                    SubjectName = reader.GetString(reader.GetOrdinal("SubjectName")),
                                    Grade = reader.GetDouble(reader.GetOrdinal("Score"))
                                });
                            }
                            return result;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public async Task<double> GetAverageBySubject(int studentID, int subjectID)
        {
            string sql = @"select Avg(Score) Average
                           from dbo.Scores
                           where StudentID = @studentID
                             and SubjectID = @subjectID";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@studentID", studentID));
                        cmd.Parameters.Add(new SqlParameter("@subjectID", subjectID));
                        con.Open();
                        return (double)(await cmd.ExecuteScalarAsync());
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Class>> GetClasses()
        {
            string sql = @"select c.ID, c.StudentID, s.FullName StudentName
                                , c.TeacherID, t.FullName TeacherName
                                , c.SubjectID, sb.[Name] SubjectName
                           from dbo.Classes c
                               join dbo.Accounts s on s.AccountType = 2 and s.ID = c.StudentID
                               join dbo.Accounts t on t.AccountType = 1 and t.ID = c.TeacherID
                               join dbo.Subjects sb on sb.ID = c.SubjectID";

            List<Class> result = new List<Class>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                result.Add(new Class
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                                    TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                    TeacherName = reader.GetString(reader.GetOrdinal("TeacherName")),
                                    SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                    SubjectName = reader.GetString(reader.GetOrdinal("SubjectName"))
                                });
                            }
                            return result;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}

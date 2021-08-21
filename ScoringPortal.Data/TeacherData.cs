using Microsoft.Data.SqlClient;
using ScoringPortal.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoringPortal.Data
{
    public class TeacherData : AccountData
    {
        public TeacherData(string masterConnectionString, string myConnectionString) : base(masterConnectionString, myConnectionString)
        {
        }

        public async Task GiveScoreToStudentForSubject(int StudentID, int TeacherID, int SubjectID, double Grade)
        {
            string sql = @"insert into dbo.Scores (StudentID, TeacherID, SubjectID, Score)
                                            values(@StudentID, @TeacherID, @SubjectID, @Score)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@StudentID", StudentID));
                        cmd.Parameters.Add(new SqlParameter("@TeacherID", TeacherID));
                        cmd.Parameters.Add(new SqlParameter("@SubjectID", SubjectID));
                        cmd.Parameters.Add(new SqlParameter("@Score", Grade));
                        con.Open();
                        await cmd.ExecuteNonQueryAsync();
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

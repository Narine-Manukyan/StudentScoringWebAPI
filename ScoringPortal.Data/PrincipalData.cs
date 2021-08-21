using Microsoft.Data.SqlClient;
using ScoringPortal.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoringPortal.Data
{
    public class PrincipalData : AccountData
    {
        public PrincipalData(string masterConnectionString, string myConnectionString) : base(masterConnectionString, myConnectionString)
        { }

        public async Task<IEnumerable<Subject>> GetAllSubjects()
        {
            string sql = @"select ID, [Name]
                            from dbo.Subjects";
            List<Subject> result = new List<Subject>();

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
                                result.Add(new Subject
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
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

        public async Task<Subject> GetSubjectByID(int id)
        {
            string sql = @"select ID, [Name] 
                           from dbo.Subjects
                           where ID = @id";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new Subject
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                };
                            }
                            return null;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public async Task AddSubject(Subject newSubject)
        {
            string sql = @"insert into dbo.Subjects(Name) values(@Name)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Name", newSubject.Name));
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

        public async Task UpdateSubject(Subject newSubject)
        {
            string sql = @"update dbo.Subjects
                           set [Name] = @Name
                           where ID = @id";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Name", newSubject.Name));
                        cmd.Parameters.Add(new SqlParameter("@id", newSubject.ID));
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

        public async Task<bool> DeleteSubject(int id)
        {
            string sql = @"delete from dbo.Subjects
                           where ID = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        con.Open();
                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Score>> GetAllScores()
        {
            string sql = @"select sc.ID, sc.StudentID, s.FullName StudentName
                                , sc.TeacherID, t.FullName TeacherName
                                , sc.SubjectID, sb.[Name] SubjectName, sc.Score
                           from dbo.Scores sc
                               join dbo.Accounts s on s.AccountType = 2 and s.ID = sc.StudentID
                               join dbo.Accounts t on t.AccountType = 1 and t.ID = sc.TeacherID
                               join dbo.Subjects sb on sb.ID = sc.SubjectID";

            List<Score> result = new List<Score>();
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

        public async Task AddClass(int StudentID, int TeacherID, int SubjectID)
        {
            string sql = @"insert into dbo.Classes(SubjectID, StudentID, TeacherID) 
                                            values(@SubjectID, @StudentID, @TeacherID)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@StudentID", StudentID));
                        cmd.Parameters.Add(new SqlParameter("@TeacherID", TeacherID));
                        cmd.Parameters.Add(new SqlParameter("@SubjectID", SubjectID));
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

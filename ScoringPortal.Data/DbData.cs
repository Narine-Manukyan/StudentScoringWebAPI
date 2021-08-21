using Microsoft.Data.SqlClient;

namespace ScoringPortal.Data
{
    public static class DbData
    {
        public static void CreateDatabaseIfNotExist(string masterConnectionString)
        {
            string sql = @"if not exists 
                           (
	                           select *
	                           from sys.databases
	                           where name = N'StudentScoringPortal'
                           )
                           begin
	                           create database StudentScoringPortal
                           end";

            try
            {
                using (SqlConnection con = new SqlConnection(masterConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public static void CreateAccountsTableIfNotExist(string connectionString)
        {
            string sql = @"if not exists 
                            (
	                            select *
	                            from sys.tables
	                            where object_id = object_id(N'dbo.Accounts')
                            )
                            begin
	                            create table dbo.Accounts
	                            (
		                            ID int identity(1, 1) not null,
                                    AccountType int not null,
		                            FullName varchar(100) not null,
		                            PhoneNumber varchar(max) not null,
		                            Email varchar(max) not null,
		                            constraint pk_Accounts_ID primary key (ID)
	                            )
                            END";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public static void CreateScoresTableIfNotExist(string connectionString)
        {
            string sql = @"if not exists 
                            (
	                            select *
	                            from sys.tables
	                            where object_id = object_id(N'dbo.Scores')
                            )
                            begin
	                            create table dbo.Scores
	                            (
		                            ID int identity(1, 1) not null,
                                    StudentID int not null,
		                            TeacherID int not null,
		                            SubjectID int not null,
		                            Score float not null,
		                            constraint pk_Scores_ID primary key (ID)
	                            )
                            end";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public static void CreateSubjectsTableIfNotExist(string connectionString)
        {
            string sql = @"if not exists 
                            (
	                            select *
	                            from sys.tables
	                            where object_id = object_id(N'dbo.Subjects')
                            )
                            begin
	                            create table dbo.Subjects
	                            (
		                            ID int identity(1, 1) not null,
                                    [Name] varchar(50) not null,
		                            constraint pk_Subjects_ID primary key (ID)
	                            )
                            end";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public static void CreateClassesTableIfNotExist(string connectionString)
        {
            string sql = @"if not exists 
                            (
	                            select *
	                            from sys.tables
	                            where object_id = object_id(N'dbo.Classes')
                            )
                            begin
	                            create table dbo.Classes
	                            (
		                            ID int identity(1, 1) not null,
                                    SubjectID int not null,
                                    StudentID int not null,
	                                TeacherID int not null,
		                            constraint pk_Classes_ID primary key (ID)
	                            )
                            end";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
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

using Microsoft.Data.SqlClient;
using ScoringPortal.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoringPortal.Data
{
    public class AccountData : IAccountData
    {
        protected readonly string connectionString;

        public AccountData(string masterConnectionString, string myConnectionString)
        {
            this.connectionString = myConnectionString;
            DbData.CreateDatabaseIfNotExist(masterConnectionString);
            DbData.CreateAccountsTableIfNotExist(this.connectionString);
            DbData.CreateScoresTableIfNotExist(this.connectionString);
            DbData.CreateSubjectsTableIfNotExist(this.connectionString);
            DbData.CreateClassesTableIfNotExist(this.connectionString);
        }

        public async Task<IEnumerable<Account>> GetAll(AccountType accountType)
        {
            string sql = @"select *
                           from dbo.Accounts
                           where AccountType = @accountType";
            List<Account> result = new List<Account>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@accountType", accountType));
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                result.Add(new Account
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    AccountType = (AccountType)reader.GetInt32(reader.GetOrdinal("AccountType")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email"))
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

        public async Task<Account> GetById(int id, AccountType accountType)
        {
            string sql = @"select ID, FullName, PhoneNumber, Email
                           from dbo.Accounts
                           where AccountType = @accountType
                             and ID = @id";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@accountType", accountType));
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new Account
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Email = reader.GetString(reader.GetOrdinal("Email"))
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

        public async Task Add(Account newAccount)
        {
            string sql = @"insert into dbo.Accounts(FullName, AccountType, PhoneNumber, Email) 
                                             values(@FullName, @AccountType, @PhoneNumber, @Email)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@FullName", newAccount.FullName));
                        cmd.Parameters.Add(new SqlParameter("@AccountType", newAccount.AccountType));
                        cmd.Parameters.Add(new SqlParameter("@PhoneNumber", newAccount.PhoneNumber));
                        cmd.Parameters.Add(new SqlParameter("@Email", newAccount.Email));
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

        public async Task Update(Account newAccount)
        {
            string sql = @"update dbo.Accounts
                           set FullName = @FullName,
                               AccountType = @AccountType,
                               PhoneNumber = @PhoneNumber,
                               Email = @Email
                           where ID = @id";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@FullName", newAccount.FullName));
                        cmd.Parameters.Add(new SqlParameter("@AccountType", newAccount.AccountType));
                        cmd.Parameters.Add(new SqlParameter("@PhoneNumber", newAccount.PhoneNumber));
                        cmd.Parameters.Add(new SqlParameter("@Email", newAccount.Email));
                        cmd.Parameters.Add(new SqlParameter("@id", newAccount.ID));
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

        public async Task<bool> Delete(int id)
        {
            string sql = @"delete from dbo.Accounts
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

    }
}

using ScoringPortal.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoringPortal.Data
{
    public interface IAccountData
    {
        Task<IEnumerable<Account>> GetAll(AccountType accountType);
        Task<Account> GetById(int id, AccountType accountType);
        Task Update(Account newAccount);
        Task Add(Account newAccount);
        Task<bool> Delete(int id);
    }
}

using Microsoft.AspNetCore.Mvc;
using ScoringPortal.Core;
using ScoringPortal.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentScoringWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "StudentSpecification")]
    public class StudentController : ControllerBase
    {
        private readonly StudentData data;

        public StudentController(StudentData data)
        {
            this.data = data;
        }

        [HttpGet]
        [Route("Students")]
        public async Task<IEnumerable<Account>> GetAll()
        {
            return await data.GetAll(AccountType.Student);
        }

        [HttpGet]
        [Route("Students/{id:int}")]
        public async Task<Account> GetById(int id)
        {
            return await data.GetById(id, AccountType.Student);
        }

        [HttpPost]
        public async Task Add(Account account)
        {   
            await data.Add(account);
        }

        [HttpPut]
        public async Task Update(Account account)
        {
            await data.Update(account);
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await data.Delete(id);
        }

        [HttpGet]
        [Route("Scores/{studentID:int}/{subjectID:int}")]
        public async Task<IEnumerable<Score>> GetScoresBySubject(int studentID, int subjectID)
        {
            return await data.GetScoresBySubject(studentID, subjectID);
        }

        [HttpGet]
        [Route("Average/{studentID:int}/{subjectID:int}")]
        public async Task<double> GetAverageBySubject(int studentID, int subjectID)
        {
            return await data.GetAverageBySubject(studentID, subjectID);
        }

        [HttpGet]
        [Route("Classes")]
        public async Task<IEnumerable<Class>> GetClasses()
        {
            return await data.GetClasses();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ScoringPortal.Core;
using ScoringPortal.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentScoringWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "PrincipalSpecification")]
    public class PrincipalController : ControllerBase
    {
        private readonly PrincipalData data;

        public PrincipalController(PrincipalData data)
        {
            this.data = data;
        }

        [HttpGet]
        [Route("Principals")]
        public async Task<IEnumerable<Account>> GetAll()
        {
            return await data.GetAll(AccountType.Principal);
        }

        [HttpGet]
        [Route("Principals/{id:int}")]
        public async Task<Account> GetById(int id)
        {
            return await data.GetById(id, AccountType.Principal);
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
        [Route("Subjects")]
        public async Task<IEnumerable<Subject>> GetAllSubjects()
        {
            return await data.GetAllSubjects();
        }

        [HttpGet]
        [Route("Subjects/{id:int}")]
        public async Task<Subject> GetSubjectByID(int id)
        {
            return await data.GetSubjectByID(id);
        }

        [HttpPost]
        [Route("NewSubject")]
        public async Task AddSubject(Subject newSubject)
        {
            await data.AddSubject(newSubject);
        }

        [HttpPut]
        [Route("UpdateSubject")]
        public async Task UpdateSubject(Subject newSubject)
        {
            await data.UpdateSubject(newSubject);
        }

        [HttpDelete]
        [Route("DeleteSubject")]
        public async Task<bool> DeleteSubject(int id)
        {
            return await data.DeleteSubject(id);
        }

        [HttpGet]
        [Route("Scores")]
        public async Task<IEnumerable<Score>> GetAllScores()
        {
            return await data.GetAllScores();
        }

        [HttpGet]
        [Route("Classes")]
        public async Task<IEnumerable<Class>> GetClasses()
        {
            return await data.GetClasses();
        }

        [HttpPost]
        [Route("Class")]
        public async Task GiveScoreToStudentForSubject(int studentID, int teacherID, int subjectID)
        {
            await data.AddClass(studentID, teacherID, subjectID);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ScoringPortal.Core;
using ScoringPortal.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentScoringWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "TeacherSpecification")]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherData data;

        public TeacherController(TeacherData data)
        {
            this.data = data;
        }

        [HttpGet]
        [Route("Teachers")]
        public async Task<IEnumerable<Account>> GetAll()
        {
            return await data.GetAll(AccountType.Teacher);
        }

        [HttpGet]
        [Route("Teachers/{id:int}")]
        public async Task<Account> GetById(int id)
        {
            return await data.GetById(id, AccountType.Teacher);
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

        [HttpPost]
        [Route("GiveScore")]
        public async Task GiveScoreToStudentForSubject(int studentID, int teacherID, int subjectID, double grade)
        {
            await data.GiveScoreToStudentForSubject(studentID, teacherID, subjectID, grade);
        }

        [HttpGet]
        [Route("Classes")]
        public async Task<IEnumerable<Class>> GetClasses()
        {
            return await data.GetClasses();
        }
    }
}

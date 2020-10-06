using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PloomesApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace PloomesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private IConfiguration configuration;

        public CaseController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [Route("GetAllCases")]
        [HttpGet]
        public string GetAllCases()
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                cases = JsonConvert.SerializeObject(db.Cases.ToList());
            }
            return cases;
        }

        [Route("GetCaseById")]
        [HttpGet]
        public string GetCaseById([FromQuery] int id)
        {
            string caseById = "";
            using (var db = new DatabaseContext(configuration))
            {
                caseById = JsonConvert.SerializeObject(db.Cases.FirstOrDefault(c => c.Id == id));
            }
            return caseById;
        }

        [Route("GetCasesByIds")]
        [HttpPost]
        public string GetCasesByIds([FromBody] JObject casesJson)
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                List<int> casesIds = casesJson["cases"].ToObject<List<Case>>().Select(c => c.Id).ToList();
                cases = JsonConvert.SerializeObject(db.Cases.Where(c => casesIds.Contains(c.Id)).ToList());
            }
            return cases;
        }

        [Route("GetCasesSubjectContains")]
        [HttpGet]
        public string GetCasesSubjectContains([FromQuery] string text)
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                cases = JsonConvert.SerializeObject(db.Cases.Where(c => c.Subject.Contains(text)).ToList());
            }
            return cases;
        }

        [Route("GetCasesDescriptionContains")]
        [HttpGet]
        public string GetCasesDescriptionContains([FromQuery] string text)
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                cases = JsonConvert.SerializeObject(db.Cases.Where(c => c.Description.Contains(text)).ToList());
            }
            return cases;
        }

        [Route("GetCasesByCustomerId")]
        [HttpGet]
        public string GetCasesByCustomerId([FromQuery] int id)
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                cases = JsonConvert.SerializeObject(db.Cases.Where(c => c.CustomerId == id).ToList());
            }
            return cases;
        }

        [Route("GetCasesByAttendantId")]
        [HttpGet]
        public string GetCasesByAttendantId([FromQuery] int id)
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                cases = JsonConvert.SerializeObject(db.Cases.Where(c => c.AttendantId == id).ToList());
            }
            return cases;
        }

        [Route("GetCasesByStatus")]
        [HttpGet]
        public string GetCasesByStatus([FromQuery] int status)
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                cases = JsonConvert.SerializeObject(db.Cases.Where(c => c.Status == status).ToList());
            }
            return cases;
        }

        [Route("GetCasesByPriority")]
        [HttpGet]
        public string GetCasesByPriority([FromQuery] int priority)
        {
            string cases = "";
            using (var db = new DatabaseContext(configuration))
            {
                cases = JsonConvert.SerializeObject(db.Cases.Where(c => c.Priority == priority).ToList());
            }
            return cases;
        }

        [Route("AddNewCase")]
        [HttpPost]
        public ActionResult AddNewCase([FromBody] JObject newCase)
        {
            Case newCaseObj = newCase.ToObject<Case>();
            using (var db = new DatabaseContext(configuration))
            {
                db.Add(newCaseObj);
                db.SaveChanges();
            }

            return Ok();
        }

        [Route("DeleteCase")]
        [HttpDelete]
        public ActionResult DeleteCase([FromBody] JObject caseJson)
        {
            Case caseObj = caseJson.ToObject<Case>();
            bool caseDeleted = false;
            using (var db = new DatabaseContext(configuration))
            {
                caseDeleted = db.Cases.Any(c => c.Id == caseObj.Id);

                if (caseDeleted)
                {
                    db.Remove(caseObj);
                    db.SaveChanges();
                }
            }

            if (caseDeleted)
                return Ok();
            else
                return NotFound();
        }

        [Route("UpdateCase")]
        [HttpPost]
        public ActionResult UpdateCase([FromBody] JObject caseToUpdate)
        {
            Case caseToUpdateObj = caseToUpdate.ToObject<Case>();
            using (var db = new DatabaseContext(configuration))
            {
                db.Update(caseToUpdateObj);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
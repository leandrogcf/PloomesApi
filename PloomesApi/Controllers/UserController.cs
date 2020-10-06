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
    public class UserController : ControllerBase
    {
        private IConfiguration configuration;

        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [Route("GetAllUsers")]
        [HttpGet]
        public string GetAllUsers()
        {
            string users = "";
            using (var db = new DatabaseContext(configuration))
            {
                users = JsonConvert.SerializeObject(db.Users.ToList());
            }
            return users;
        }

        [Route("userById")]
        [HttpGet]
        public string GetUserById([FromQuery] int id)
        {
            string user = "";
            using (var db = new DatabaseContext(configuration))
            {
                user = JsonConvert.SerializeObject(db.Users.FirstOrDefault(u => u.Id == id));
            }
            return user;
        }

        [Route("usersByIds")]
        [HttpPost]
        public string GetUsersByIds([FromBody] JObject usersJson)
        {
            string users = "";
            using (var db = new DatabaseContext(configuration))
            {
                List<int> usersIds = usersJson["users"].ToObject<List<User>>().Select(u => u.Id).ToList();
                users = JsonConvert.SerializeObject(db.Users.Where(u => usersIds.Contains(u.Id)).ToList());
            }
            return users;
        }
        [Route("/searchLogin")]
        [HttpGet]
        public string GetUserByLogin([FromQuery] string login)
        {
            string user = "";
            using (var db = new DatabaseContext(configuration))
            {
                user = JsonConvert.SerializeObject(db.Users.FirstOrDefault(u => u.Login == login));
            }
            return user;
        }

        [Route("usersByType")]
        [HttpGet]
        public string GetUsersByType([FromQuery] int type)
        {
            string users = "";
            using (var db = new DatabaseContext(configuration))
            {
                users = JsonConvert.SerializeObject(db.Users.Where(u => u.Type == type).ToList());
            }
            return users;
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public ActionResult DeleteUser([FromBody] JObject userJson)
        {
            User user = userJson.ToObject<User>();
            bool userDeleted = false;
            using (var db = new DatabaseContext(configuration))
            {
                userDeleted = db.Users.Any(u => u.Id == user.Id || u.Login == user.Login);

                if (userDeleted)
                {
                    db.Remove(user);
                    db.SaveChanges();
                }
            }

            if (userDeleted)
                return Ok();
            else
                return NotFound();
        }

        [Route("AddNewUser")]
        [HttpPost]
        public ActionResult AddNewUser([FromBody] JObject newUser)
        {
            User newUserObj = newUser.ToObject<User>();

            using (var db = new DatabaseContext(configuration))
            {
                db.Add(newUserObj);
                db.SaveChanges();
            }

            return Ok();
        }

        [Route("UpdateUser")]
        [HttpPost]
        public ActionResult UpdateUser([FromBody] JObject updateUser)
        {
            User user = updateUser.ToObject<User>();
            using (var db = new DatabaseContext(configuration))
            {
                db.Update(user);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
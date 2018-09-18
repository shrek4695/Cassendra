using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameAPI.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        
        [HttpPost]
        [Logging]
        public String RegisterUser([FromBody]UserDetails value)
        {
            Database db = new Database();
            return(db.CreateUser(value.FirstName, value.LastName, value.UserName));
        }
        
    }
}

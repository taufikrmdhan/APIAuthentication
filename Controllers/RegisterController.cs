using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using APIAuthentication.Models;
using APIAuthentication.Utils;
using System.Text;

namespace APIAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        dbXamarinContext dbXamarinContext = new dbXamarinContext();

        [HttpPost]
        public string Post([FromBody]TbUser value)
        {
            if (!dbXamarinContext.TbUsers.Any(User => User.Username.Equals(value.Username)))
            {
                TbUser user = new TbUser();
                user.Username = value.Username;
                user.Salt = Convert.ToBase64String(Common.GetRandomSalt(16));
                user.Password = Convert.ToBase64String(Common.SaltHashPassword(
                    Encoding.ASCII.GetBytes(value.Password),
                    Convert.FromBase64String(user.Salt)));

                try
                {
                    dbXamarinContext.Add(user);
                    dbXamarinContext.SaveChanges();
                    return JsonConvert.SerializeObject("Register Successfully");
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(ex.Message);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("User is Existing in Database");
            }
        }
    }
}

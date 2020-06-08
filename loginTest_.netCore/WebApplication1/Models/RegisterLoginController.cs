using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisterLoginController : ControllerBase
    {
        public static Dictionary<string, string> namePwd;

        [HttpPost]
        [ActionName("Register")]
        public string Register([FromBody] dynamic value)
        {
            Init();
            if (namePwd.ContainsKey(value.name.ToString()))
            {
                return RetrunMessage(1, "already exit this user name");
            }
            namePwd.Add(value.name.ToString(), MD5(value.pwd.ToString()));
            return RetrunMessage(0, "register successful");

        }


        [HttpPost]
        [ActionName("Login")]
        public string Login([FromBody] dynamic value)
        {
            Init();
            if (!namePwd.ContainsKey(value.name.ToString())||MD5(value.pwd.ToString())!=namePwd[value.name.ToString()])
            {
                return RetrunMessage(1, "name or password error");
            }
           
            return RetrunMessage(0, "login successful");

        }

        private void Init()
        {
            if (namePwd == null)
            {
                namePwd = new Dictionary<string, string>();
            }
        }

        private string RetrunMessage(int code,string message)
        {
            ReturnCode rc = new ReturnCode();
            rc.code = code;
            rc.message = message;
            return JsonConvert.SerializeObject(rc);
        }

        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)));
            t2 = t2.Replace("-", "").ToLower();
            return t2;
        }
    }
}
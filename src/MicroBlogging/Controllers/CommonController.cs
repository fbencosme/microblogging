using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MicroBlogging.Models;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace MicroBlogging.Controllers
{
    public class CommonController : Controller
    {
        [FromServices]
        public MicroBloggerContext DbContext { get; set; }

        [FromServices]
        public IHostingEnvironment HostingEnvironment { get; set; }

        protected IActionResult Json<T>(T obj)
        {
            if (obj == null)
                return new HttpNotFoundObjectResult("entity not found");
            return new JsonResult(obj);
        }

        protected IActionResult BadRequest(string msg)
        {
            return new BadRequestObjectResult(msg);
        }

        protected string EncodePassword(string password)
        {
            var salt = "encod09737834!@#%$(*&34034ingpassword12309^1323";
            var bytes = Encoding.Unicode.GetBytes(password);
            var src = Encoding.Unicode.GetBytes(salt);
            var dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            var algorithm = HashAlgorithm.Create("SHA1");
            var inarray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inarray);
        }

        protected int Save<T>(DbSet<T> dbSet, T obj) where T : User
        {
            try
            {
                obj.Password = EncodePassword(obj.Password);
                DbContext.Users.Add(obj);
                return DbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return 0;
            }
        }

        protected User UserSession()
        {
            var userId = HttpContext.Session.GetInt32("user");
            if (userId.HasValue)
            {
                return DbContext.Users.FirstOrDefault(u => u.Id == userId);
            }
            return null;
        }

        protected int UserIdSession()
        {
            var userId = HttpContext.Session.GetInt32("user");
            if (userId.HasValue)
            {
                return userId.Value;
            }
            return -1;
        }
    }
}
using System.IO;
using System.Linq;
using MicroBlogging.Filters;
using MicroBlogging.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;

namespace MicroBlogging.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : CommonController
    {
        [ValidateAttributes]
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Signup([FromBody] User user)
        {
            if (Save(DbContext.Users, user) == 1)
            {
                CreateSession(user);
                RemovePassword(user);
                return Json(user);
            }
            return BadRequest("EmailUsed");
        }

        [ValidateAttributes]
        [HttpPost("[action]")]
        public IActionResult Signin([FromBody] LoginUser user)
        {
            var pwd = EncodePassword(user.Password);
            var dbUser = DbContext.Users.FirstOrDefault(u =>
                u.Username == user.Username && u.Password == pwd
                );
            CreateSession(dbUser);
            RemovePassword(dbUser);
            return Json(dbUser);
        }

        [HttpGet]
        [Authenticate]
        public IActionResult List()
        {
            var userId = HttpContext.Session.GetInt32("user");

            var follows = (
                from   f in DbContext.Follows
                where  f.Source == userId.Value
                select f.Target
            ).ToList();

            var users = (
                from  u in DbContext.Users
                where u.Id != userId.Value
                select new
                {
                    Id        = u.Id,
                    FullName  = u.FullName,
                    Username  = u.Username,
                    Gender    = u.Gender,
                    Picture   = u.Picture,
                    Following = follows.Any(f => f == u.Id)
                });
            return Json(users);
        }

        [HttpGet("[action]")]
        [Authenticate]
        public IActionResult Current()
        {
            var userId = HttpContext.Session.GetInt32("user");
            if (userId.HasValue)
            {
                var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
                RemovePassword(user);
                return Json(user);
            }
            return new HttpUnauthorizedResult();
        }

        [HttpPost("[action]")]
        [Authenticate]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }

        [ValidateAttributes]
        [HttpPost("[action]")]
        public IActionResult UploadPicture()
        {
            var file = HttpContext.Request.Form.Files.FirstOrDefault();
            if (file == null)
                return new BadRequestObjectResult("no image found to save");

            var fileName = ContentDispositionHeaderValue
                .Parse(file.ContentDisposition)
                .FileName
                .Trim('"');

            var filePath = Path.Combine(HostingEnvironment.WebRootPath, "pictures", fileName);

            file.SaveAs(filePath);

            using (var ctx = DbContext)
            {
                var user = (
                    from s in ctx.Users
                    where s.Id == UserIdSession()
                    select s
                    ).FirstOrDefault();

                user.Picture = fileName;
                ctx.SaveChanges();
                return Json(fileName);
            }
        }

        private void CreateSession(User user)
        {
            if (user != null)
                HttpContext.Session.SetInt32("user", user.Id);
        }

        private void RemovePassword(User user)
        {
            if (user != null) user.Password = null;
        }

        [ValidateAttributes]
        [HttpPost("[action]")]
        public IActionResult Follow([FromBody] FollowRequest follow)
        {
           var dbFollow = DbContext.Follows.Add(new Follow
           {
               Source = follow.Source ,
               Target = follow.Target 
           });

           DbContext.SaveChanges();
           return Json(dbFollow);
        }

        [ValidateAttributes]
        [HttpPost("[action]")]
        public IActionResult UnFollow([FromBody] FollowRequest follow)
        {
            DbContext.Follows.RemoveRange(DbContext.Follows.Where( f => 
              (f.Source == follow.Source && f.Target == follow.Target)
            ));
            DbContext.SaveChanges();
            return Ok();
        }
    }
}
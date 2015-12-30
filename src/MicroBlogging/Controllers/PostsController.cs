using System;
using System.IO;
using System.Linq;
using MicroBlogging.Filters;
using MicroBlogging.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Net.Http.Headers;

namespace MicroBlogging.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : CommonController
    {
        [HttpPost]
        [ValidateAttributes]
        [Authenticate]
        public IActionResult Create([FromBody] Post post)
        {
            AddUser(post);
            AddDate(post);
            DbContext.Posts.Add(post);
            DbContext.SaveChanges();
            return Json(post);
        }

        [HttpGet]
        [ValidateAttributes]
        [Authenticate]
        public IActionResult Get()
        {
            var userId = HttpContext.Session.GetInt32("user");
            if (userId.HasValue)
            {
                var follows = (
                   from  f in DbContext.Follows
                   where f.Source == userId
                   select f.Target
                ).ToList();
                follows.Add(userId.Value);

                return Json(
                    DbContext.Posts
                        .OrderByDescending(p => p.Date)
                        .Include(p => p.User)
                        .Where(p   => follows.Exists( f => f.Equals(p.User.Id)) ));
            }
            return Ok();
        }

        [ValidateAttributes]
        [HttpPost("[action]")]
        public IActionResult UploadPicture()
        {
            var form = HttpContext.Request.Form;
            var file = HttpContext.Request.Form.Files.FirstOrDefault();
            var postId = int.Parse(HttpContext.Request.Form["postId"]);
            if (file == null)
                return new BadRequestObjectResult("no image found to save");

            var fileName = ContentDispositionHeaderValue
                .Parse(file.ContentDisposition)
                .FileName
                .Trim('"');

            var filePath = Path.Combine(HostingEnvironment.WebRootPath, "posts", fileName);

            file.SaveAs(filePath);

            using (var ctx = DbContext)
            {
                var post = (
                    from s in ctx.Posts
                    where s.Id == postId
                    select s
                    ).FirstOrDefault();

                post.Picture = fileName;
                ctx.SaveChanges();

                return Json(fileName);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        private void AddUser(Post post)
        {
            var userId = HttpContext.Session.GetInt32("user");
            if (userId.HasValue)
            {
                post.User = DbContext.Users.FirstOrDefault(u => u.Id == userId.Value);
            }
        }

        private void AddDate(Post post)
        {
            post.Date = DateTime.Now;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HotNews.Core;
using HotNews.Core.Objects;
using HotNews.Models;
using HotNews.Providers;
using Newtonsoft.Json;


namespace HotNews.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAuthProvider _authProvider;
        private readonly IBlogRepository _blogRepository;

        public AdminController(IAuthProvider authProvider, IBlogRepository blogRepository=null)
        {
            _authProvider = authProvider;
            _blogRepository = blogRepository;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (_authProvider.IsLoggedIn)
                return RedirectToUrl(returnUrl);

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && _authProvider.Login(model.UserName, model.Password))
            {
                return RedirectToUrl(returnUrl);
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Logout()
        {
            _authProvider.Logout();

            return RedirectToAction("Login", "Admin");
        }

        private ActionResult RedirectToUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Manage");
            }
        }

       

        public ContentResult Posts(JqInViewModel jqParams)
        {
            var posts = _blogRepository.Posts(jqParams.page - 1, jqParams.rows,
                jqParams.sidx, jqParams.sord == "asc");

            var totalPosts = _blogRepository.TotalPosts(false);

            return Content(JsonConvert.SerializeObject(new
            {
                page = jqParams.page,
                records = totalPosts,
                rows = posts,
                total = Math.Ceiling(Convert.ToDouble(totalPosts) / jqParams.rows)
            }, new CustomDateTimeConverter()), "application/json");
        }

        [HttpPost, ValidateInput(false)]
        public ContentResult AddPost(Post post)
        {
            string json;

            ModelState.Clear();

            if (TryValidateModel(post))
            {
                var id = _blogRepository.AddPost(post);

                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Post added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the post."
                });
            }

            return Content(json, "application/json");
        }

        public ContentResult GetCategoriesHtml()
        {
            var categories = _blogRepository.Categories().OrderBy(s => s.Name);

            var sb = new StringBuilder();
            sb.AppendLine(@"<select>");

            foreach (var category in categories)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>",
                    category.Id, category.Name));
            }

            sb.AppendLine("<select>");
            return Content(sb.ToString(), "text/html");
        }

        public ContentResult GetTagsHtml()
        {
            var tags = _blogRepository.Tags().OrderBy(s => s.Name);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<select multiple=""multiple"">");

            foreach (var tag in tags)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>",
                    tag.Id, tag.Name));
            }

            sb.AppendLine("<select>");
            return Content(sb.ToString(), "text/html");
        }

    }
}
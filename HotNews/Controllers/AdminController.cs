﻿using HotNews.Core;
using HotNews.Core.Objects;
using HotNews.Models;
using HotNews.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HotNews.ViewModels;

namespace HotNews.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAuthProvider _authProvider;
        private readonly IBlogRepository _blogRepository;

        public AdminController(IAuthProvider authProvider, IBlogRepository blogRepository = null)
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
            //var url = HttpRuntime.AppDomainAppVirtualPath;
            //var url =  ConfigurationManager.AppSettings["BlogUrl"];
            //return View(new UploadViewModel
            //{
            //     baseUrl = url,
               
            //});

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

        [HttpPost, ValidateInput(false)]
        public ContentResult EditPost(Post post)
        {
            string json;

            ModelState.Clear();

            if (TryValidateModel(post))
            {
                _blogRepository.EditPost(post);
                json = JsonConvert.SerializeObject(new
                {
                    id = post.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeletePost(int id)
        {
            _blogRepository.DeletePost(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Post deleted successfully."
            });

            return Content(json, "application/json");
        }

        public ContentResult Categories()
        {
            var categories = _blogRepository.Categories();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = categories.Count,
                rows = categories,
                total = 1
            }), "application/json");
        }

        [HttpPost]
        public ContentResult AddCategory([Bind(Exclude = "Id")]Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                var id = _blogRepository.AddCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Category added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the category."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult EditCategory(Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogRepository.EditCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = category.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeleteCategory(int id)
        {
            _blogRepository.DeleteCategory(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Category deleted successfully."
            });

            return Content(json, "application/json");
        }

        public ContentResult Tags()
        {
            var tags = _blogRepository.Tags();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = tags.Count,
                rows = tags,
                total = 1
            }), "application/json");
        }

        [HttpPost]
        public ContentResult AddTag([Bind(Exclude = "Id")]Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                var id = _blogRepository.AddTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Tag added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the tag."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult EditTag(Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogRepository.EditTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = tag.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeleteTag(int id)
        {
            _blogRepository.DeleteTag(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Tag deleted successfully."
            });

            return Content(json, "application/json");
        }

        public ActionResult GoToPost(int id)
        {
            var post = _blogRepository.Post(id);
            return RedirectToRoute(new { controller = "Blog", action = "Post", year = post.PostedOn.Year, month = post.PostedOn.Month, title = post.UrlSlug });
        }

        //[HttpPost]
        //public JsonResult UploadImage(HttpPostedFileBase Image)
        //{
        //    //string directory = @"C:\gaming\uploads\";

        //    //if (Image != null && Image.ContentLength > 0)
        //    //{
        //    //    var fileName = Path.GetFileName(Image.FileName);
        //    //    Image.SaveAs(Path.Combine(directory, fileName));
        //    //}

        //    if (Image != null && Image.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(Image.FileName);
        //        var path = Path.Combine(Server.MapPath("Uploads"), fileName);
        //        Image.SaveAs(path);



        //    }

        //    return Json(new { isUploaded = true, message = "Uploaded Successfully" }, "text/html");
        //}

        //public ActionResult Upload(string id)
        //{
        //  //  var url = HttpRuntime.AppDomainAppVirtualPath;
        //    return View(new UploadViewModel
        //    {
        //       // baseUrl = url,
        //        track = id
        //    });
        //}

        //[HttpPost]
        //public ActionResult UploadFile()
        //{
        //    var file = Request.Files[0];
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);
        //        var path = Path.Combine(Server.MapPath("~/App_Files/Upload"), fileName);
        //        file.SaveAs(path);
        //    }

        //    return RedirectToAction("Manage");
        //}

        //public ActionResult ListFiles()
        //{
        //    var fileData = new List<ViewDataUploadFileResults>();

        //    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/App_Files/Upload"));
        //    if (dir.Exists)
        //    {
        //        string[] extensions = MimeTypes.ImageMimeTypes.Keys.ToArray();

        //        FileInfo[] files = dir.EnumerateFiles()
        //                              .Where(f => extensions.Contains(f.Extension.ToLower()))
        //                              .ToArray();

        //        if (files.Length > 0)
        //        {
        //            foreach (FileInfo file in files)
        //            {
        //                var relativePath = VirtualPathUtility.ToAbsolute("~/App_Files/Upload") + "/" + file.Name;

        //                fileData.Add(new ViewDataUploadFileResults()
        //                {
        //                    url = relativePath,
        //                    name = file.Name,
        //                    type = MimeTypes.ImageMimeTypes[file.Extension],
        //                    size = Convert.ToInt32(file.Length)
        //                });
        //            }
        //        }
        //    }

        //    return Json(fileData, JsonRequestBehavior.AllowGet);
        //}



    }
}
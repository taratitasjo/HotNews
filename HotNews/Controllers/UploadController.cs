using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotNews.ViewModels;

namespace HotNews.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/
        public ActionResult Index(string id)
        {

            //var url = HttpRuntime.AppDomainAppVirtualPath;
            var url = ConfigurationManager.AppSettings["BlogUrl"];
            return View(new UploadViewModel
            {
                baseUrl = url,
                track = id
                });
        }


        [HttpPost]
        public ActionResult UploadFile()
        {
            var file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Files/Upload"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ListFiles()
        {
            var fileData = new List<ViewDataUploadFileResults>();

            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/App_Files/Upload"));
            if (dir.Exists)
            {
                string[] extensions = MimeTypes.ImageMimeTypes.Keys.ToArray();

                FileInfo[] files = dir.EnumerateFiles()
                                      .Where(f => extensions.Contains(f.Extension.ToLower()))
                                      .ToArray();

                if (files.Length > 0)
                {
                    foreach (FileInfo file in files)
                    {
                       // var baseurl = ConfigurationManager.AppSettings["BlogUrl"];
                        var relativePath = VirtualPathUtility.ToAbsolute("~/App_Files/Upload") + "/" + file.Name;

                        fileData.Add(new ViewDataUploadFileResults()
                        {
                           // url = baseurl+relativePath,
                            url = relativePath,
                            name = file.Name,
                            type = MimeTypes.ImageMimeTypes[file.Extension],
                            size = Convert.ToInt32(file.Length)
                        });
                    }
                }
            }

            return Json(fileData, JsonRequestBehavior.AllowGet);
        }
    }
}

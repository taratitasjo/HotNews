﻿using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel.Syndication;
using System.Web;
using HotNews.Core;
using HotNews.Core.Objects;
using HotNews.Models;
using System.Web.Mvc;
using System.Text;

namespace HotNews.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult Posts(int p = 1)
        {
            // todo:read and return posts from repositor

            var viewModel = new ListViewModel(_blogRepository, p);

            ViewBag.Title = "Latest Posts";
            return View("List", viewModel);
        }
        public ViewResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""",
                viewModel.Category.Name);
            return View("List", viewModel);
        }

        public ViewResult Tag(string tag, int p = 1)
        {
            // TODO: get the posts for the tag and return the view.

            var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""",
                viewModel.Tag.Name);
            return View("List", viewModel);

        }
        public ViewResult Search(string s, int p = 1)
        {
            ViewBag.Title = String.Format(@"Lists of posts found
                        for search text ""{0}""", s);

            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);
            return View("List", viewModel);
        }

        public ViewResult Post(int year, int month, string title)
        {
            var post = _blogRepository.Post(year, month, title);

            if (post == null)
                throw new HttpException(404, "Post not found");

            if (post.Published == false && User.Identity.IsAuthenticated == false)
                throw new HttpException(401, "The post is not published");

            return View(post);
        }


        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var widgetViewModel = new WidgetViewModel(_blogRepository);
            return PartialView("_Sidebars", widgetViewModel);
        }

        public ActionResult Feed()
        {
            var blogTitle = ConfigurationManager.AppSettings["BlogTitle"];
            var blogDescription = ConfigurationManager.AppSettings["BlogDescription"];
            var blogUrl = ConfigurationManager.AppSettings["BlogUrl"];

            // Create a collection of SyndicationItemobjects from the latest posts
            var posts = _blogRepository.Posts(0, 25).Select
            (
              p => new SyndicationItem
                  (
                      p.Title,
                      p.Description,
                      new Uri(string.Concat(blogUrl, p.Href(Url)))
                  )
            );

            // Create an instance of SyndicationFeed class passing the SyndicationItem collection
            var feed = new SyndicationFeed(blogTitle, blogDescription, new Uri(blogUrl), posts)
            {
                Copyright = new TextSyndicationContent(String.Format("Copyright (c) {0}", blogTitle)),
                Language = "en-US"
            };

            // Format feed in RSS format through Rss20FeedFormatter formatter
            var feedFormatter = new Rss20FeedFormatter(feed);

            // Call the custom action that write the feed to the response
            return new FeedResult(feedFormatter);
        }


        //Edw kanoume dokimi gia sfalamata arkei ston browser na grapseis badaction kai meta
        //boreis na deis to la8os sto elmah.axd
        //public ActionResult BadAction()
        //{
        //    throw new Exception("You forgot to implement this action!");
        //}
        public ViewResult Contact()
        {
            return View();
        }


        [HttpPost]
        public ViewResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                using (var client = new SmtpClient())
                {
                    var adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
                    var from = new MailAddress(adminEmail, "JustBlog Messenger");
                    var to = new MailAddress(adminEmail, "JustBlog Admin");

                    using (var message = new MailMessage(from, to))
                    {
                        message.Body = contact.Body;
                        message.IsBodyHtml = true;
                        message.BodyEncoding = Encoding.UTF8;

                        message.Subject = contact.Subject;
                        message.SubjectEncoding = Encoding.UTF8;

                        message.ReplyTo = new MailAddress(contact.Email);

                        client.Send(message);
                    }
                }

                return View("Thanks");
            }

            return View();
        }

        public ViewResult Aboutme()
        {
            return View();
        }
    }
}
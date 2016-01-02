using HotNews.Core.Objects;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HotNews
{
    public static class ActionLinkExtensions
    {
        public static MvcHtmlString PostLink(this HtmlHelper helper, Post post)
        {
            return helper.ActionLink
            (
                     post.Title,     // anchor text
                    "Post",         // action name
                    "Blog",         // controller name
                     new
                        {           // route parameters
                          year = post.PostedOn.Year,
                          month = post.PostedOn.Month,
                          title = post.UrlSlug
                        },
                     new
                        {           // html attributes
                       title = post.Title
                         }
            );
        }

        public static MvcHtmlString CategoryLink(this HtmlHelper helper,
            Category category)
        {
            return helper.ActionLink(category.Name, "Category", "Blog",
                new
                {
                    category = category.UrlSlug
                },
                new
                {
                    title = String.Format("See all posts in {0}", category.Name)
                });
        }

        public static MvcHtmlString TagLink(this HtmlHelper helper, Tag tag)
        {
            return helper.ActionLink(tag.Name, "Tag", "Blog", new { tag = tag.UrlSlug },
                new
                {
                    title = String.Format("See all posts in {0}", tag.Name)
                });
        }
    }
}
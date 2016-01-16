using System;
using System.Web.Mvc;
using System.Configuration;
using HotNews.Core.Objects;


namespace HotNews
{
    public static class Extensions
    {
        public static string ToConfigLocalTime(this DateTime utcDT)
        {
            var istTZ = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["Timezone"]);
            return String.Format("{0} ({1})", TimeZoneInfo.ConvertTimeFromUtc(utcDT, istTZ).ToShortDateString(), ConfigurationManager.AppSettings["TimezoneAbbr"]);
        }


        public static string Href(this Post post, UrlHelper helper)
        {
            return helper.RouteUrl(new { controller = "Blog", action = "Post", year = post.PostedOn.Year, month = post.PostedOn.Month, title = post.UrlSlug });
        }
    }
}

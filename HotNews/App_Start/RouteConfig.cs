using System.Web.Mvc;
using System.Web.Routing;

namespace HotNews
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                       "Post",
                       "Archive/{year}/{month}/{title}",
                   new { controller = "Blog", action = "Post" }
                      );

            routes.MapRoute(
                        "Archive",
                         "Archive/{year}/{month}",
                    new { controller = "Blog", action = "Archive", year = UrlParameter.Optional, month = UrlParameter.Optional }
                         );


            routes.MapRoute(
                     "Category",
                     "Category/{category}",
                    new { controller = "Blog", action = "Category" }
                            );

            routes.MapRoute(
                         "Tag",
                         "Tag/{tag}",
                    new { controller = "Blog", action = "Tag" }
                         );


            routes.MapRoute(
                        "Login",
                         "Login",
                     new { controller = "Admin", action = "Login" }
                        );

            routes.MapRoute(
                         "Logout",
                         "Logout",
                    new { controller = "Admin", action = "Logout" }
                        );

            routes.MapRoute(
                        "Manage",
                         "Manage",
                     new { controller = "Admin", action = "Manage" }
                        );

            routes.MapRoute(
                      "AdminAction",
                      "Admin/{action}",
                    new { controller = "Admin", action = "Login" }
                         );

            routes.MapRoute(
                    "Action",
                    "{action}",
                    new { controller = "Blog", action = "Posts" }
                    );

         //   routes.MapRoute(
         //     "Upload",// Route name
         //      "Admin/{action}/{id}", // URL with parameters
         //    new { controller = "Admin", action = "Upload", id = UrlParameter.Optional } // Parameter defaults
         //);

            routes.MapRoute(
              "Upload",// Route name
               "Upload/{action}/{id}", // URL with parameters
             new { controller = "Upload", action = "Upload", id = UrlParameter.Optional } // Parameter defaults
         );

            //     routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Admin", action = "Upload", id = UrlParameter.Optional } // Parameter defaults
            //);

            //routes.MapRoute(
            //           "UploadImage",
            //            "Admin/UploadImage",
            //        new { controller = "Admin", action = "UploadImage" }
            //           );

            //routes.MapRoute(
            //     "Default",
            //     "{controller}/{action}/{id}",
            //       new { controller = "Blog", action = "Posts", id = UrlParameter.Optional }
            //         );
        }
    }
}
using HotNews.Core;
using Ninject;
using Ninject.Web.Common;
using System.Web.Routing;
using HotNews.Providers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HotNews.Core.Objects;


namespace HotNews
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            kernel.Load(new RepositoryModule());
            kernel.Bind<IBlogRepository>().To<BlogRepository>();
            kernel.Bind<IAuthProvider>().To<AuthProvider>();

            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(Post), new PostModelBinder(Kernel));
            base.OnApplicationStarted();
        }
    }
}
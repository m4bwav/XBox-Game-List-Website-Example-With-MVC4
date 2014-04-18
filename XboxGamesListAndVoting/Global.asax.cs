using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;

namespace XboxGamesListAndVoting
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            InitializeDependencyInjection();

            ExecuteDefaultWebSiteInitialization();
        }

        private static void ExecuteDefaultWebSiteInitialization()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void InitializeDependencyInjection()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof (MvcApplication).Assembly);
            builder.Register(x => ((MvcHandler)HttpContext.Current.Handler).RequestContext).InstancePerHttpRequest();
            builder.Register(x => new UrlHelper(x.Resolve<RequestContext>())).InstancePerHttpRequest();

            var webAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(webAssembly)
                .Where(x => x.Namespace != null && x.Namespace.StartsWith("XboxGamesListAndVoting.Services"))
                .AsImplementedInterfaces();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace ProyectoISO700
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //WebSecurity.InitializeDatabaseConnection("BolsaEmpleoContext", "Users", "UserId", "UserName", autoCreateTables: true);
            WebSecurity.InitializeDatabaseConnection("SECURITYWEB", "UserProfile", "UserId", "Username", true);
        }
    }
}

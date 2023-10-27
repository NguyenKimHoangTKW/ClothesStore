using ClotheSstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClotheSstore.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            UserAdmin admin = (UserAdmin)HttpContext.Current.Session["Admin"];
            if (admin != null)
            {
                return;
            }
            else
            {
                var returnURL = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Error", area = "", returnURL = returnURL.ToString() }));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MejjHonda.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            dynamic factura = new ExpandoObject();
            return View();
        }

    }
}
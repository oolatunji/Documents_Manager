using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManager.Web.Controllers
{
    public class PhysicalLocationController : Controller
    {
        // GET: PhysicalLocation
        public ActionResult AddLocation()
        {
            return View();
        }

        public ActionResult ViewLocation()
        {
            return View();
        }

        public ActionResult UpdateLocation()
        {
            return View();
        }
    }
}
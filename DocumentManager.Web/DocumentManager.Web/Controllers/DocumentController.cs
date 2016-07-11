using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManager.Web.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult UploadDocument()
        {
            return View();
        }

        public ActionResult ViewDocument()
        {
            return View();
        }

        public ActionResult RequestDocument()
        {
            return View();
        }

        public ActionResult ApproveDocument()
        {
            return View();
        }

        public ActionResult Transactions()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
    }
}
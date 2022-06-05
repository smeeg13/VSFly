using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class AdminController : Controller
    {

        private readonly IVSFlyServices _vSFly;

        public AdminController(IVSFlyServices vSFly)
        {
            _vSFly = vSFly;
        }


        // GET: AdminController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserType") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

       
    }
}

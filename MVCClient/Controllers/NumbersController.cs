using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class NumbersController : Controller
    {
        // GET: NumbersController
        public ActionResult Index()
        {
            return View();
        }

    }
}

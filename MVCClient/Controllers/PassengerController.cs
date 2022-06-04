using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Services;
using MVCClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class PassengerController : Controller
    {

        private readonly ILogger<PassengerController> _logger;
        private readonly IVSFlyServices _vSFly;

        public PassengerController(ILogger<PassengerController> logger, IVSFlyServices vSFly)
        {
            _logger = logger;
            _vSFly = vSFly;
        }

        // GET: PassengerController
       [HttpGet]
        public async Task<ActionResult> Index()
        {
            var listPassengers = await _vSFly.GetPassengers();
            return View(listPassengers);
        }


        // GET: PassengerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PassengerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PassengerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection formCollection)
        {
            PassengerM employee = new PassengerM();
            if (ModelState.IsValid)
            {

                employee.FullName = formCollection["FullName"];
                employee.PassportID = formCollection["PassportID"];
                employee.Email = formCollection["Email"];
                employee.Birthday = Convert.ToDateTime(formCollection["Birthday"]);


                var statusCode = _vSFly.CreatePassenger(employee);
                if (statusCode)
                {
                    //Creation of the Passenger is OK
                    return RedirectToAction("Index");
                }
            }


            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(employee);
        }

        // GET: PassengerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PassengerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PassengerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PassengerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

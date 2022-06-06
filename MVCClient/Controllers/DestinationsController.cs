using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Models;
using MVCClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class DestinationsController : Controller
    {

        private readonly IVSFlyServices _vSFly;

        public DestinationsController(IVSFlyServices vSFly)
        {

            _vSFly = vSFly;
        }

        // GET: DestinationsController
        public async  Task<IActionResult> Index()
        {
            
              var destination = await _vSFly.GetAllDestinations();

            return View(destination);
        }

        // GET: DestinationsController/Details/5
        public ActionResult Details(int destinationName)
        {
            return View();
        }

        // GET: DestinationsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DestinationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: DestinationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DestinationsController/Edit/5
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

        // GET: DestinationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DestinationsController/Delete/5
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

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
    public class LoginController : Controller
    {
        private readonly IVSFlyServices _vSFly;

        public LoginController(IVSFlyServices vSFly)
        {
            _vSFly = vSFly;
        }


        // GET: LoginController
        public async Task<ActionResult> Index(Login login)
        {

            if (ModelState.IsValid)
            {
                //Redirect to Pilot page if Pilot
                if (login.IsPilot)
                {
                    var pilot = await _vSFly.GetPilotByPassportID(login.PassportID);
                    if (pilot != null)
                    {
                        if (login.Email.Equals(pilot.Email))
                        {
                            HttpContext.Session.SetInt32("PersonId", pilot.PersonId);
                            HttpContext.Session.SetString("UserType", "Pilot");

                            return RedirectToAction("Index", "Pilots");
                        }
                            
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Email or Passport ID");
                    return View();
                }

                var passenger = await _vSFly.GetPassengerByPassportID(login.PassportID);

                //Redirect to Passenger page if Passenger
                if (passenger != null)
                {
                    if (login.Email.Equals(passenger.Email))
                    {
                        if (passenger.Status.Equals("Admin"))
                        {
                            HttpContext.Session.SetString("UserType", "Admin");
                            HttpContext.Session.SetInt32("PersonId", passenger.PersonId);

                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            HttpContext.Session.SetString("UserType", "Passenger");
                            HttpContext.Session.SetInt32("PersonId", passenger.PersonId);

                            return RedirectToAction("Index", "Passenger");
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Id or Email");
            }
            
            return View();
        }
    }
}

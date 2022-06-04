﻿using Microsoft.AspNetCore.Http;
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
            {
                if (ModelState.IsValid)
                {
                    var passenger = await _vSFly.GetPassenger(login.PersonID);

                    //Redirect to Passenger page if Passenger
                    if (passenger != null)
                    { 
                        if (passenger.Email.Equals(login.Email))
                        {
                            if (passenger.Email.Equals("megane.solliard@hotmail.com"))
                            {
                                HttpContext.Session.SetString("UserType", "Admin");
                                HttpContext.Session.SetInt32("PersonId", passenger.PersonId);

                                return RedirectToAction("Index", "Admin");

                            }
                            else
                            {
                            HttpContext.Session.SetString("UserType", "Passenger");
                                HttpContext.Session.SetInt32("PersonId", passenger.PersonId);

                                return RedirectToAction("Index", "Home");

                            }
                        }

                        ModelState.AddModelError(string.Empty, "Invalid Id or Email");

                        HttpContext.Session.SetInt32("PersonId", passenger.PersonId);
                        return RedirectToAction("Index", "Home");
                    }
                  
                     if (passenger == null)
                                        {
                                           var pilot = await _vSFly.GetPilot(login.PersonID);
//Redirect to Pilot page if Pilot
                    if (pilot != null)
                    {
                        HttpContext.Session.SetInt32("PersonId", pilot.PersonId);
                        HttpContext.Session.SetString("UserType", "Pilot");

                        return RedirectToAction("Index", "Pilots");
                    }
}
                }
            }
            return View();
        }

    }
}

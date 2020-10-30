﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sekemin.Data;
using Sekemin.Models;
using Sekemin.ViewModels;

namespace Sekemin.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<Osoba> userManager;
        private readonly BazaContext context;
        public double broj = 0;
        public AccountController(ILogger<AccountController> logger, RoleManager<IdentityRole> roleManager, UserManager<Osoba> userManager, BazaContext context)
        {
            _logger = logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var korisnik =  await GetKorisnik();
            if( korisnik == null)
            {
                if (korisnik.Uloga == Uloga.Administrator)
                    return View();
            }
               

            return View();
        }

        private async Task<Osoba> GetKorisnik()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }


        [HttpGet]
        public IActionResult KreirajUlogu()
        {
            return View();
        }

        /// <summary>
        /// komentar :)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> KreirajUlogu(KreirajUlogu model)
        {
            if( ModelState.IsValid)
            {
                IdentityRole uloga = new IdentityRole
                {
                    Name = model.NazivUloge
                };
                roleManager.Dispose();
                IdentityResult result = await roleManager.CreateAsync(uloga);

                if( result.Succeeded)
                {
                    return RedirectToAction("index", "account");
                }
            }

            return View(model);
        }

        [HttpPost] 
        public async Task DodijeliUlogu()
        {
            // dodjela
            var korisnik = await GetKorisnik();
            /* dodavanje */
            await userManager.AddToRoleAsync(korisnik, "Administrator");
        }

        
    }
}
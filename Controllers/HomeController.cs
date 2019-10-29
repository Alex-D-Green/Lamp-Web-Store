using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LampWebStore.Views.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LampWebStore.Controllers
{
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Show some error's info but without sensitive information like stack trace etc.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
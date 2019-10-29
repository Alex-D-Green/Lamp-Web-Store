using System.Diagnostics;
using System.Threading.Tasks;

using LampWebStore.Models;
using LampWebStore.Views.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LampWebStore.Controllers
{
    public class HomeController: Controller
    {
        /// <summary>DB context to work with lamps store DB.</summary>
        private readonly LampsContext db;


        public HomeController(LampsContext db) //DI...
        {
            this.db = db;
        }


        /// <summary>
        /// Welcome page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of the products.
        /// </summary>
        /// <remarks>It's async method cuz interaction with the DB could be quite time-consuming.</remarks>
        public async Task<IActionResult> Products()
        {
            Lamp[] lamps = await db.Lamps
                                   .AsNoTracking() //To cut off waste of resources
                                   .ToArrayAsync();

            return View(lamps);
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
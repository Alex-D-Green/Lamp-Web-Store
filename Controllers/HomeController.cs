using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using LampWebStore.Models;
using LampWebStore.Views.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LampWebStore.Controllers
{
    public class HomeController: Controller
    {
        //Constants with names of the fields that are available for sorting
        public const string SortByLampType = "LampType";
        public const string SortByManufacturer = "Manufacturer";
        public const string SortByCost = "Cost";

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
        /// <param name="sortingProp">The property by which products will be sorted (or <c>null</c>).</param>
        /// <param name="sortByAsc">Sort products in ascending order.</param>
        public async Task<IActionResult> Products(string sortingProp, bool sortByAsc = true)
        {
            IQueryable<Lamp> ret = db.Lamps;

            switch(sortingProp)
            {
                case SortByLampType:
                    ret = sortByAsc ? ret.OrderBy(o => o.LampType) : ret.OrderByDescending(o => o.LampType); 
                    break;

                case SortByManufacturer:
                    ret = sortByAsc ? ret.OrderBy(o => o.Manufacturer) : ret.OrderByDescending(o => o.Manufacturer);
                    break;

                case SortByCost:
                    ret = sortByAsc ? ret.OrderBy(o => o.Cost) : ret.OrderByDescending(o => o.Cost);
                    break;

                case "":
                case null: 
                    break; //Sorting is not needed

                default:
                    throw new ArgumentException($"Inappropriate value {sortingProp}.", nameof(sortingProp));
            }

            return View(new ProductsViewModel { 
                Lamps = await ret.AsNoTracking().ToArrayAsync(), 
                SortingProp = sortingProp,
                SortByAsc = sortByAsc
            });
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
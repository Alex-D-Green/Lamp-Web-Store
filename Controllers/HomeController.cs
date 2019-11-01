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

        /// <summary>The amount of items per page.</summary>
        public const int ItemsPerPage = 4;


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
        /// <param name="page">The current page to display.</param>
        /// <param name="sortingProp">The property by which products will be sorted (or <c>null</c>).</param>
        /// <param name="sortByAsc">Sort products in ascending order.</param>
        public async Task<IActionResult> Products(int page, string sortingProp, bool sortByAsc = true)
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
                    return BadRequest($"Inappropriate value {sortingProp}.");
            }

            int itemsCount = await ret.CountAsync();
            int totalPages = (int)Math.Ceiling((double)itemsCount / ItemsPerPage);

            if(page < 0 || page >= totalPages)
                return BadRequest($"The value should be equal or greater than 0 and less than {totalPages}.");

            return View(new ProductsViewModel { 
                Lamps = await ret.Skip(page * ItemsPerPage)
                                 .Take(ItemsPerPage)
                                 .AsNoTracking()
                                 .ToArrayAsync(), 
                SortingProp = sortingProp,
                SortByAsc = sortByAsc,
                TotalPages = totalPages,
                CurrentPage = page
            });
        }

        /// <summary>
        /// Get method, just show the empty form.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post method, check if lamp's data is ok and add new entity into the DB.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] //To prevent handling Post requests from the foreign apps
        public async Task<IActionResult> Create(LampViewModel lamp)
        {
            if(!ModelState.IsValid)
                return View(lamp); //There are some validation errors, reshow form

            //Here could be used AutoMapper or something like this...
            db.Lamps.Add(new Lamp {
                //lamp.Id is ignored here
                Cost = lamp.Cost,
                ImageRef = lamp.ImageRef,
                LampType = lamp.LampType,
                Manufacturer = lamp.Manufacturer
            });

            await db.SaveChangesAsync(); //DB/ORM exceptions probably should being handled in a special way...

            return RedirectToAction("Products");
        }

        /// <summary>
        /// Show the deletion confirmation form.
        /// </summary>
        [ActionName("Delete")] //To use same name for both Get and Post requests
        public async Task<ActionResult> ConfirmDelete(int id)
        {
            if(id <= 0)
                return BadRequest($"Inappropriate id ({id}).");

            Lamp item = await db.Lamps.FirstOrDefaultAsync(o => o.Id == id);

            if(item is null)
                return BadRequest($"Item with id = {id} was not found.");

            return View(item);
        }

        /// <summary>
        /// Delete the item.
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Lamp item = await db.Lamps.FirstOrDefaultAsync(o => o.Id == id);

            if(item is null)
                return BadRequest($"Item with id = {id} was not found.");

            db.Remove(item);
            await db.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        /// <summary>
        /// Get method, just show the form with data.
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
                return BadRequest($"Inappropriate id ({id}).");

            Lamp item = await db.Lamps.FirstOrDefaultAsync(o => o.Id == id);

            if(item is null)
                return BadRequest($"Item with id = {id} was not found.");

            //Here could be used AutoMapper or something like this...
            return View(new LampViewModel {
                Id = item.Id,
                Cost = item.Cost,
                ImageRef = item.ImageRef,
                LampType = item.LampType,
                Manufacturer = item.Manufacturer
            });
        }

        /// <summary>
        /// Post method, check if lamp's data is ok and change entity into the DB.
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LampViewModel lamp)
        {
            if(!ModelState.IsValid)
                return View(lamp); //There are some validation errors, reshow form

            Lamp item = await db.Lamps.FirstOrDefaultAsync(o => o.Id == lamp.Id);

            if(item is null)
                return BadRequest($"Item with id = {lamp.Id} was not found.");

            item.Cost = lamp.Cost;
            item.ImageRef = lamp.ImageRef;
            item.LampType = lamp.LampType;
            item.Manufacturer = lamp.Manufacturer;

            db.Lamps.Update(item);
            await db.SaveChangesAsync(); //DB/ORM exceptions probably should being handled in a special way...

            return RedirectToAction("Products");
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
using LampWebStore.Views.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LampWebStore.Components
{
    /// <summary>
    /// The example of view component with some params and separate html view.
    /// It adds pagination controls into the page.
    /// </summary>
    public sealed class Pagination: ViewComponent
    {
        public IViewComponentResult Invoke(ProductsViewModel model, string controller, string action)
        {
            //Pass some params through ViewData
            ViewData.Add("Controller", controller);
            ViewData.Add("Action", action);

            //And some through model
            return View(model);
        }
    }
}

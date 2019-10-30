using System.Collections.Generic;

using LampWebStore.Models;

namespace LampWebStore.Views.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<Lamp> Lamps { get; set; }

        public string SortingProp { get; set; }

        public bool SortByAsc { get; set; }
    }
}

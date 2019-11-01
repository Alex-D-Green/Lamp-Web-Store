using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using LampWebStore.Models;


namespace LampWebStore.Views.ViewModels
{
    /// <summary>
    /// Lamp view model that used when need to add new lamp entity or change the existing one.
    /// </summary>
    public class LampViewModel
    {
        //I use the same view model for adding and editing actions although the Id field is redundant 
        //for adding. It's probably better to have two different view models for these actions.
        public int Id { get; set; }

        [DisplayName("Lamp type")]
        public LampType LampType { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0, 2000)]
        [DisplayName("Cost")]
        public double Cost { get; set; }

        [DataType(DataType.ImageUrl)]
        [StringLength(500)]
        [DisplayName("Image link")]
        public string ImageRef { get; set; }
    }
}

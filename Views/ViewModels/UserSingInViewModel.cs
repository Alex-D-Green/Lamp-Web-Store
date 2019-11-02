using System.ComponentModel.DataAnnotations;

namespace LampWebStore.Views.ViewModels
{
    /// <summary>
    /// This view model is used for user's authentication.
    /// </summary>
    public class UserSingInViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LampWebStore.Views.ViewModels
{
    /// <summary>
    /// This view model is used for creating a new user account.
    /// </summary>
    public class UserSingUpViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Login { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }

        [DisplayName("Password confirmation")]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}

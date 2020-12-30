using System.ComponentModel.DataAnnotations;
namespace Exercise.Data.ModelView
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(50,MinimumLength =8)]
        public string Password { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string ConfirmPassword { get; set; }
    }
}
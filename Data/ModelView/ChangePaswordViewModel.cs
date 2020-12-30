using System.ComponentModel.DataAnnotations;
namespace Exercise.Data.ModelView
{
    public class ChangePaswordViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string OldPassword { get; set; }
        public string Id { get; set; }
    }
}
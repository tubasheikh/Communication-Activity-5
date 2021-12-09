using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//This import, helps us perform the data annotations you see below.
using System.ComponentModel.DataAnnotations;


namespace Lab5_1_.Models
{
    public class ResetPasswordViewModel
    {
        // the rest password model has email, password, conf password and token
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}

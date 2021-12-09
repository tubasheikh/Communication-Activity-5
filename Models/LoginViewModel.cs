using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//This import, helps us perform the data annotations you see below.
using System.ComponentModel.DataAnnotations;

namespace Lab5_1_.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}

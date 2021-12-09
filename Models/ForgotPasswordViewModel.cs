using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//This import, helps us perform the data annotations you see below.
using System.ComponentModel.DataAnnotations;


namespace Lab5_1_.Models
{
    public class ForgotPasswordViewModel
    {
        // This model only requires an email address to reset the password 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

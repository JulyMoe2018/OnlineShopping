using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class Register
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name: ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name: ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address: ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        [Display(Name = "Telephone: ")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City: ")]
        public int City { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [StringLength(150)]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 5)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password: ")]
        public string ConfirmPassword { get; set; }
    }
}
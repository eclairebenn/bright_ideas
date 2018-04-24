using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace bright_ideas.Models
{
    public class RegisterViewModel : BaseEntity
    {

        [Required]
        [MinLength(2)]
        [Display(Name = "Name")]
        public string Name {get; set;}

        [Required]
        [MinLength(2)]
        [Display(Name = "Alias")]
        public string Alias {get; set;}


        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [Required]
        [MinLength(8)]
        [Compare("Password", ErrorMessage="Password and Password Confirmation must match")]
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        public string PasswordConf {get;set;}

    }

}
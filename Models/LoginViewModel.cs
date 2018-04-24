using System;
using System.ComponentModel.DataAnnotations;

namespace bright_ideas.Models
{
    public class LoginViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name="Email")]
        public string LogEmail {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        
        public string LogPassword {get;set;}

    }
}
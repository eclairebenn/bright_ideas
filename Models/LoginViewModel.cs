using System;
using System.ComponentModel.DataAnnotations;

namespace dojo_activities.Models
{
    public class LoginViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name="Email")]
        public string LogEmail {get;set;}

        [Required(ErrorMessage="Invalid Password")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        
        public string LogPassword {get;set;}

    }
}
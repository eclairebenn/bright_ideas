using System;
using System.ComponentModel.DataAnnotations;

namespace dojo_activities.Models
{
    public class LoginViewModel : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required(ErrorMessage="Invalid Password")]
        [DataType(DataType.Password)]
        
        public string Password {get;set;}

    }
}
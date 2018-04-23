using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dojo_activities.Models
{
    public class MeetViewModel : BaseEntity
    {
        public int MeetId {get;set;}

        [Required]
        [MinLength(2)]
        public string Title {get; set;}

        [Required]
        [MinLength(10)]
        public string Description {get;set;}

        
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time {get;set;}

        [Required]
        public int Duration {get;set;}
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Participant> Participants {get;set;}

        public MeetViewModel()
        {
            Participants = new List<Participant>();

        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime dateValue = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second);
            if (dateValue < DateTime.Now)
            {
                yield return new ValidationResult(
                    $"Please enter a Date and Time in the future.");
            }
        }
    }    
}
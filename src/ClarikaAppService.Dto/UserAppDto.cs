using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClarikaAppService.Dto
{

    public class UserAppDto
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateBirth { get; set; }
        public int? Age
        {
            get
            {
                if (DateBirth.HasValue)
                {
                    // Calculate age based on DateBirth
                    int age = DateTime.Now.Year - DateBirth.Value.Year;
                    if (DateTime.Now.Month < DateBirth.Value.Month || (DateTime.Now.Month == DateBirth.Value.Month && DateTime.Now.Day < DateBirth.Value.Day))
                    {
                        age--;
                    }
                    return age;
                }
                return null;
            }
            set
            {
                // You can choose to set the Age property directly, or you can leave the setter empty.
            }
        }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public long? CountryId { get; set; }
        public CountryDto Country { get; set; }

        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}

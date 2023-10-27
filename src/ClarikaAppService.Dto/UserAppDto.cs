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
        public int? Age { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public long? CountryId { get; set; }
        public CountryDto Country { get; set; }

        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}

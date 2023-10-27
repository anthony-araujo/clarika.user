using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClarikaAppService.Dto
{

    public class UserLocationDto
    {
        public long Id { get; set; }
        [Required]
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Province { get; set; }
        public long? CountryId { get; set; }
        public CountryDto Country { get; set; }
        public long? LocationTypeId { get; set; }
        public LocationTypeDto LocationType { get; set; }
        public long? UserAppId { get; set; }
        public UserAppDto UserApp { get; set; }

        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}

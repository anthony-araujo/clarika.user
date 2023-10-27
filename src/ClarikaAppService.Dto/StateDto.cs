using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClarikaAppService.Dto
{

    public class StateDto
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public long? CountryId { get; set; }
        public CountryDto Country { get; set; }

        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClarikaAppService.Dto
{

    public class CityDto
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public long? StateId { get; set; }
        public StateDto State { get; set; }

        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}

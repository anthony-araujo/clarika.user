using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClarikaAppService.Domain.Entities
{
    [Table("city")]
    public class City : BaseEntity<long>
    {
        [Required]
        public string Name { get; set; }
        public long? StateId { get; set; }
        public State State { get; set; }

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var city = obj as City;
            if (city?.Id == null || city?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, city.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "City{" +
                    $"ID='{Id}'" +
                    $", Name='{Name}'" +
                    "}";
        }
    }
}

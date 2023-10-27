using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClarikaAppService.Domain.Entities
{
    [Table("state")]
    public class State : BaseEntity<long>
    {
        [Required]
        public string Name { get; set; }
        public IList<City> Cities { get; set; } = new List<City>();
        public long? CountryId { get; set; }
        public Country Country { get; set; }

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var state = obj as State;
            if (state?.Id == null || state?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, state.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "State{" +
                    $"ID='{Id}'" +
                    $", Name='{Name}'" +
                    "}";
        }
    }
}

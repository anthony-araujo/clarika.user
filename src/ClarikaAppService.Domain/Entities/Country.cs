using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClarikaAppService.Domain.Entities
{
    [Table("country")]
    public class Country : BaseEntity<long>
    {
        [Required]
        public string Name { get; set; }
        public IList<State> States { get; set; } = new List<State>();
        public IList<UserLocation> UserLocations { get; set; } = new List<UserLocation>();
        public IList<UserApp> UserApps { get; set; } = new List<UserApp>();

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var country = obj as Country;
            if (country?.Id == null || country?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, country.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "Country{" +
                    $"ID='{Id}'" +
                    $", Name='{Name}'" +
                    "}";
        }
    }
}

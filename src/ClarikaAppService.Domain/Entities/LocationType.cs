using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClarikaAppService.Domain.Entities
{
    [Table("location_type")]
    public class LocationType : BaseEntity<long>
    {
        [Required]
        public string Name { get; set; }
        public IList<UserLocation> UserLocations { get; set; } = new List<UserLocation>();

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var locationType = obj as LocationType;
            if (locationType?.Id == null || locationType?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, locationType.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "LocationType{" +
                    $"ID='{Id}'" +
                    $", Name='{Name}'" +
                    "}";
        }
    }
}

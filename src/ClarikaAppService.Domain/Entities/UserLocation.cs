using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClarikaAppService.Domain.Entities
{
    [Table("user_location")]
    public class UserLocation : BaseEntity<long>
    {
        [Required]
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Province { get; set; }
        public long? CountryId { get; set; }
        public Country Country { get; set; }
        public long? LocationTypeId { get; set; }
        public LocationType LocationType { get; set; }
        public long? UserAppId { get; set; }
        public UserApp UserApp { get; set; }

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var userLocation = obj as UserLocation;
            if (userLocation?.Id == null || userLocation?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, userLocation.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "UserLocation{" +
                    $"ID='{Id}'" +
                    $", Address='{Address}'" +
                    $", ZipCode='{ZipCode}'" +
                    $", Province='{Province}'" +
                    "}";
        }
    }
}

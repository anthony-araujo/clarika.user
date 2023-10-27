using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClarikaAppService.Domain.Entities
{
    [Table("user_app")]
    public class UserApp : BaseEntity<long>
    {
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
        public IList<UserLocation> UserLocations { get; set; } = new List<UserLocation>();
        public long? CountryId { get; set; }
        public Country Country { get; set; }

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var userApp = obj as UserApp;
            if (userApp?.Id == null || userApp?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, userApp.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "UserApp{" +
                    $"ID='{Id}'" +
                    $", FirstName='{FirstName}'" +
                    $", LastName='{LastName}'" +
                    $", Email='{Email}'" +
                    $", DateBirth='{DateBirth}'" +
                    $", Age='{Age}'" +
                    $", PasswordHash='{PasswordHash}'" +
                    $", SecurityStamp='{SecurityStamp}'" +
                    $", ConcurrencyStamp='{ConcurrencyStamp}'" +
                    "}";
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BloodProvider.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string FullName { get; set; }
        public string BloodType { get; set; }
        public bool IsDonor { get; set; }
        public bool IsHospital { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

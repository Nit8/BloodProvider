using Microsoft.AspNetCore.Identity;

namespace BloodProvider.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string BloodType { get; set; }
        public bool IsDonor { get; set; }
        public bool IsHospital { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

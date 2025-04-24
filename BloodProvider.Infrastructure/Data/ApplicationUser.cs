using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodProvider.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string BloodType { get; set; }
        public string Address { get; set; }
        public bool IsDonor { get; set; }
        public bool IsHospital { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}


using System.ComponentModel.DataAnnotations.Schema;

namespace BloodProvider.Core.Entities
{
    public class BloodRequest
    {
        public int Id { get; set; }
        public string BloodType { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Hospital")]
        public string HospitalId { get; set; }
        public virtual ApplicationUser Hospital { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public bool IsUrgent { get; set; }
        public RequestStatus Status { get; set; }
    }

    public enum RequestStatus { Pending, Fulfilled }
}

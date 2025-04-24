using BloodProvider.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodProvider.Core.Interfaces
{
    public interface IDonorService
    {
        Task<List<ApplicationUser>> FindCompatibleDonorsAsync(string bloodType, double latitude, double longitude, int radiusKm = 50);
        Task<List<BloodRequest>> GetPendingRequestsNearDonorAsync(string donorId);
    }
}

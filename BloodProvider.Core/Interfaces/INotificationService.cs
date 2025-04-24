using BloodProvider.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodProvider.Core.Interfaces
{
    public interface INotificationService
    {
        Task SendBloodRequestNotificationAsync(BloodRequest request, List<string> donorIds);
        Task SendDonationConfirmationAsync(string donorId, string requestId);
        Task SendEmergencyAlertAsync(string bloodType, string location);
    }
}

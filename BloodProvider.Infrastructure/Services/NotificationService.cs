//using BloodProvider.Core.Entities;
//using BloodProvider.Core.Interfaces;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.Extensions.Logging;

//namespace BloodProvider.Infrastructure.Services
//{
//    public class NotificationService : INotificationService
//    {
//        private readonly IHubContext<NotificationHub> _hubContext;
//        private readonly AppDbContext _context;
//        private readonly IEmailService _emailService;
//        private readonly ILogger<NotificationService> _logger;

//        public NotificationService(
//            IHubContext<NotificationHub> hubContext,
//            AppDbContext context,
//            IEmailService emailService,
//            ILogger<NotificationService> logger)
//        {
//            _hubContext = hubContext;
//            _context = context;
//            _emailService = emailService;
//            _logger = logger;
//        }

//        public async Task SendBloodRequestNotificationAsync(BloodRequest request, List<string> donorIds)
//        {
//            try
//            {
//                _logger.LogInformation($"Sending notifications for request {request.Id} to {donorIds.Count} donors");

//                var hospital = await _context.Users.FindAsync(request.HospitalId);
//                var message = $"URGENT: {request.BloodType} needed at {hospital?.FullName ?? "Hospital"}";

//                // Real-time SignalR notifications
//                foreach (var donorId in donorIds)
//                {
//                    await _hubContext.Clients.User(donorId)
//                        .SendAsync("ReceiveBloodRequest", new
//                        {
//                            RequestId = request.Id,
//                            BloodType = request.BloodType,
//                            Hospital = hospital?.FullName,
//                            IsUrgent = request.IsUrgent,
//                            RequestDate = request.RequestDate
//                        });
//                }

//                // Email fallback notifications
//                var donors = await _context.Users
//                    .Where(u => donorIds.Contains(u.Id))
//                    .ToListAsync();

//                foreach (var donor in donors)
//                {
//                    await _emailService.SendEmailAsync(
//                        donor.Email,
//                        "Blood Request Alert",
//                        $"{message}\n\nPlease check the BloodLink app for details.");
//                }

//                _logger.LogInformation($"Successfully sent notifications for request {request.Id}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Failed to send notifications for request {request.Id}");
//                throw;
//            }
//        }

//        public async Task SendDonationConfirmationAsync(string donorId, string requestId)
//        {
//            try
//            {
//                var donor = await _context.Users.FindAsync(donorId);
//                var request = await _context.BloodRequests.FindAsync(requestId);

//                if (donor == null || request == null)
//                {
//                    _logger.LogWarning($"Donor {donorId} or request {requestId} not found");
//                    return;
//                }

//                // SignalR notification
//                await _hubContext.Clients.User(donorId)
//                    .SendAsync("ReceiveDonationConfirmation", new
//                    {
//                        RequestId = request.Id,
//                        BloodType = request.BloodType,
//                        Hospital = request.Hospital?.FullName,
//                        DonationDate = DateTime.UtcNow
//                    });

//                // Email confirmation
//                await _emailService.SendEmailAsync(
//                    donor.Email,
//                    "Donation Confirmation",
//                    $"Thank you for your donation!\n\n" +
//                    $"Request: {request.BloodType} for {request.Hospital?.FullName}\n" +
//                    $"Date: {DateTime.UtcNow:yyyy-MM-dd}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error sending confirmation to donor {donorId}");
//                throw;
//            }
//        }

//        public async Task SendEmergencyAlertAsync(string bloodType, string location)
//        {
//            try
//            {
//                _logger.LogInformation($"Sending emergency alert for {bloodType} in {location}");

//                var compatibleDonors = await _context.Users
//                    .Where(u => u.IsDonor && u.BloodType == bloodType)
//                    .ToListAsync();

//                foreach (var donor in compatibleDonors)
//                {
//                    await _hubContext.Clients.User(donor.Id)
//                        .SendAsync("ReceiveEmergencyAlert", new
//                        {
//                            BloodType = bloodType,
//                            Location = location,
//                            AlertDate = DateTime.UtcNow
//                        });
//                }

//                _logger.LogInformation($"Sent emergency alert to {compatibleDonors.Count} donors");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error sending emergency alerts");
//                throw;
//            }
//        }
//    }
//}

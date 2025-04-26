using BloodProvider.Core.Entities;
using BloodProvider.Core.Interfaces;
using BloodProvider.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodProvider.Infrastructure.Services
{
    public class DonorMatchingService : IDonorService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DonorMatchingService> _logger;

        public DonorMatchingService(AppDbContext context, ILogger<DonorMatchingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ApplicationUser>> FindCompatibleDonorsAsync(
            string bloodType,
            double latitude,
            double longitude,
            int radiusKm = 50)
        {
            try
            {
                _logger.LogInformation($"Finding compatible donors for blood type {bloodType}");

                var compatibleTypes = GetCompatibleBloodTypes(bloodType);

                // Get all potential donors from database asynchronously
                var potentialDonors = await _context.Users
                    .Where(u => u.IsDonor && compatibleTypes.Contains(u.BloodType))
                    .AsNoTracking()
                    .ToListAsync();

                // Filter and sort in memory
                var matchedDonors = potentialDonors
                    .Where(u => CalculateDistance(latitude, longitude, u.Latitude, u.Longitude) <= radiusKm)
                    .OrderBy(u => CalculateDistance(latitude, longitude, u.Latitude, u.Longitude))
                    .Take(100)
                    .ToList();

                _logger.LogInformation($"Found {matchedDonors.Count} compatible donors");
                return matchedDonors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding compatible donors");
                throw;
            }
        }

        public async Task<List<BloodRequest>> GetPendingRequestsNearDonorAsync(string donorId)
        {
            try
            {
                var donor = await _context.Users.FindAsync(donorId);
                if (donor == null)
                {
                    _logger.LogWarning($"Donor {donorId} not found");
                    return new List<BloodRequest>();
                }

                // Get all pending requests with hospital data
                var requests = await _context.BloodRequests
                    .Include(r => r.Hospital)
                    .Where(r => r.Status == RequestStatus.Pending)
                    .AsNoTracking()
                    .ToListAsync();

                // Filter by distance and sort
                return requests
                    .Where(r => CalculateDistance(
                        donor.Latitude, donor.Longitude,
                        r.Hospital.Latitude, r.Hospital.Longitude) <= 50)
                    .OrderByDescending(r => r.IsUrgent)
                    .ThenBy(r => r.RequestDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting requests for donor {donorId}");
                throw;
            }
        }

        private List<string> GetCompatibleBloodTypes(string bloodType)
        {
            // Implement real blood type compatibility logic
            return bloodType switch
            {
                "O-" => new List<string> { "O-" },
                "O+" => new List<string> { "O+", "O-" },
                // ... other blood types
                _ => new List<string> { bloodType }
            };
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Haversine formula implementation
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return 6371 * c; // Distance in km
        }
    }
}

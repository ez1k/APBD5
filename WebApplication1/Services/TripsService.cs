using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;


namespace WebApplication1.Services
{
    public interface ITripsService
    {
        public Task<ICollection<TripAllData>> GetAllTripData();
       
    }
    public class TripsService : ITripsService
    {
        private readonly Master1Context _context;

        public TripsService(Master1Context context)
        {
            _context = context;
        }

        public async Task<ICollection<TripAllData>> GetAllTripData()
        {
            return await _context.Trips.Select(trip => new TripAllData
            {
                Name = trip.Name,
                Description = trip.Description,
                DateFrom = trip.DateFrom,
                DateTo = trip.DateTo,
                MaxPeople = trip.MaxPeople,
                Clients = trip.ClientTrips.Select(clientTrip => new TripAllDataClient
                {
                    FirstName = clientTrip.IdClientNavigation.FirstName,
                    LastName = clientTrip.IdClientNavigation.LastName,
                }).ToList(),
                Countries = trip.IdCountries.Select(country => new TripAllDataCountry
                {
                    Name = country.Name,

                }).ToList()
            }).OrderByDescending(e => e.DateFrom).ToListAsync();
        }



    }
}

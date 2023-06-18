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
        public Task Remove(Client client);
        public Task<bool> DoesClientExists(int id);
        public Task<int> GetMax();
        public Task<Client?> GetClient(int id);

        public Task<bool> DoesClientExistsPesel(string pesel);
        public Task Add(int id, ClientTripAssign clientTripAssign);
        public Task<Client?> GetClientByPesel(string pesel);
        public Task<bool> DoesClientExistsTrip(int clientid, ClientTripAssign clientTripAssign);
        public Task<bool> DoesExistsTrip(ClientTripAssign clientTripAssign);
        public Task AddAsync(int clientid, ClientTripAssign clientTripAssign);
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

        public async Task<bool> DoesClientExists(int id)
        {
            return await _context.Clients.AnyAsync(e => e.IdClient == id); ;
        }

        public async Task<Client?> GetClient(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(e => e.IdClient == id);
        }
        public async Task<Client?> GetClientByPesel(string pesel)
        {
            return await _context.Clients.FirstOrDefaultAsync(e => e.Pesel == pesel);
        }

        public async Task<bool> DoesClientExistsTrip(int clientid, ClientTripAssign clientTripAssign)
        {
            return await _context.ClientTrips.AnyAsync(e => e.IdClient == clientid && e.IdTrip == clientTripAssign.tripID);
        }
        public async Task<bool> DoesExistsTrip(ClientTripAssign clientTripAssign)
        {
            return await _context.Trips.AnyAsync(e => e.IdTrip == clientTripAssign.tripID);
        }
        public async Task<bool> DoesClientExistsPesel(string pesel)
        {
            return await _context.Clients.AnyAsync(e => e.Pesel == pesel);
        }

        public async Task<int> GetMax()
        {
            return _context.Clients.Max(e => e.IdClient) + 1;
        }

        public async Task Add(int id, ClientTripAssign clientTripAssign)
        {
            await _context.Clients.AddAsync(new Client
            {
                IdClient = id,
                FirstName = clientTripAssign.firstName,
                LastName = clientTripAssign.lastName,
                Email = clientTripAssign.email,
                Telephone = clientTripAssign.telephone,
                Pesel = clientTripAssign.pesel,
            });
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(int clientid, ClientTripAssign clientTripAssign)
        {
            await _context.AddAsync(new ClientTrip
            {
                IdClient = clientid,
                IdTrip = clientTripAssign.tripID,
                PaymentDate = clientTripAssign.PaymentDate.HasValue ? null : clientTripAssign.PaymentDate,
                RegisteredAt = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Client client)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

    }



    }


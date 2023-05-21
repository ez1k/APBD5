using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly Master1Context _context;

        public ClientsController (Master1Context context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Remove(int id)
        {
            if(await _context.ClientTrips.SingleOrDefaultAsync(e => e.IdClient == id) != null)
            {
                return Conflict();
            }

            var client = await _context.Clients.SingleOrDefaultAsync(e => e.IdClient == id);
            if (client == null)
            {
                return NotFound();
            }
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(ClientTripAssign clientTripAssign)
        {
            int clientid = -1;
            if (!await _context.Clients.AnyAsync(e => e.Pesel == clientTripAssign.pesel))
            {
                clientid = _context.Clients.Max(e => e.IdClient) + 1;
                await _context.Clients.AddAsync(new Client
                {
                    IdClient = clientid,
                    FirstName = clientTripAssign.firstName,
                    LastName = clientTripAssign.lastName,
                    Email = clientTripAssign.email,
                    Telephone = clientTripAssign.telephone,
                    Pesel = clientTripAssign.pesel,
                });
            }
            await _context.SaveChangesAsync();
            if (clientid == -1)
            {
                var client = await _context.Clients.SingleOrDefaultAsync(e => e.Pesel == clientTripAssign.pesel);
                clientid = client.IdClient;
            }


            if (await _context.ClientTrips.AnyAsync(e => e.IdClient == clientid && e.IdTrip == clientTripAssign.tripID))
            {
                return Conflict();
            }

            if (!await _context.Trips.AnyAsync(e => e.IdTrip == clientTripAssign.tripID))
            {
                return NotFound();
            }

            await _context.AddAsync(new ClientTrip
            {
                IdClient = clientid,
                IdTrip = clientTripAssign.tripID,
                PaymentDate = clientTripAssign.PaymentDate.HasValue ? null : clientTripAssign.PaymentDate,
                RegisteredAt = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}

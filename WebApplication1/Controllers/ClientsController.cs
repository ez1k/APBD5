using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ITripsService _dbService;

        public ClientsController (TripsService context)
        {
            _dbService = context;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Remove(int id)
        {
            if(await _dbService.DoesClientExists(id))
            {
                return Conflict();
            }

            var client = await _dbService.GetClient(id);
            if (client == null)
            {
                return NotFound();
            }
            _dbService.Remove(client);
            return Ok();
        }
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(ClientTripAssign clientTripAssign)
        {
            int clientid = -1;

            if (!await _dbService.DoesClientExistsPesel(clientTripAssign.pesel))
            {
                clientid = await _dbService.GetMax();
                await _dbService.Add(clientid, clientTripAssign);

            }
            if (clientid == -1)
            {
                var client = await _dbService.GetClientByPesel(clientTripAssign.pesel);
                clientid = client.IdClient;
            }


            if (await _dbService.DoesClientExistsTrip(clientid, clientTripAssign))
            {
                return Conflict();
            }

            if (!await _dbService.DoesExistsTrip(clientTripAssign))
            {
                return NotFound();
            }

            await _dbService.AddAsync(clientid, clientTripAssign);
            return NoContent();

        }
    }
}

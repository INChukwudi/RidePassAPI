using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RidePassAPI.Data;
using RidePassAPI.Dtos.Passenger;
using RidePassAPI.Extensions.MappingExtensions;
using RidePassAPI.Models;
using RidePassAPI.Models.IdentityModels;
using RidePassAPI.Responses;

namespace RidePassAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly RidePassAPIContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PassengersController(RidePassAPIContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Passengers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passenger>>> GetPassenger()
        {
          if (_context.Passenger == null)
          {
              return NotFound();
          }
            return await _context.Passenger.ToListAsync();
        }

        // GET: api/Passengers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Passenger>> GetPassenger(int id)
        {
          if (_context.Passenger == null)
          {
              return NotFound();
          }
            var passenger = await _context.Passenger.FindAsync(id);

            if (passenger == null)
            {
                return NotFound();
            }

            return passenger;
        }

        // PUT: api/Passengers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassenger(int id, Passenger passenger)
        {
            if (id != passenger.Id)
            {
                return BadRequest();
            }

            _context.Entry(passenger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Passengers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Passenger>> PostPassenger(RegisterPassengerDto passengerDto)
        {
            if (_context.Passenger == null)
            {
                return Problem("Entity set 'RidePassAPIContext.Passenger'  is null.");
            }

            var passenger = new Passenger().PassengerFromRegisterPassengerDto(passengerDto);
            _context.Passenger.Add(passenger);
            await _context.SaveChangesAsync();

            var user = new AppUser
            {
                UserId = passenger.Id,
                UserType = (int?)UserTypes.Customer,
                Email = passenger.Email.ToLower().Trim(),
                PhoneNumber = passenger.PhoneNumber,
                UserName = passenger.Username.ToLower().Trim(),
            };

            var result = await _userManager.CreateAsync(user, passengerDto.Password!);
            if (!result.Succeeded)
            {
                _context.Passenger.Remove(passenger);
                await _context.SaveChangesAsync();
                return BadRequest(new ErrorResponse(400, result.Errors.First().Description));
            }

            return Ok(new SuccessResponse(200, "Passenger created successfully!"));
            //return CreatedAtAction("GetPassenger", new { id = passenger.Id }, passenger);
        }

        // DELETE: api/Passengers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            if (_context.Passenger == null)
            {
                return NotFound();
            }
            var passenger = await _context.Passenger.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }

            _context.Passenger.Remove(passenger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PassengerExists(int id)
        {
            return (_context.Passenger?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

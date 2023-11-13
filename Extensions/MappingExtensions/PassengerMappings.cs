using RidePassAPI.Dtos.Passenger;
using RidePassAPI.Models;

namespace RidePassAPI.Extensions.MappingExtensions
{
    public static class PassengerMappings
    {
        public static Passenger PassengerFromRegisterPassengerDto(this Passenger passenger, RegisterPassengerDto passengerDto)
        {
            return new Passenger
            {
                Email = passengerDto.Email,
                PhoneNumber = passengerDto.PhoneNumber,
                FirstName = passengerDto.FirstName,
                LastName = passengerDto.LastName,
                Username = passengerDto.Username,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now,
            };
        }
    }
}

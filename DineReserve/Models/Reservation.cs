using System;
using System.ComponentModel.DataAnnotations;

namespace DineReserve.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime ReservationTime { get; set; }

        [Range(1, 20)]
        public int NumberOfGuests { get; set; }

        public string Status { get; set; } = "Confirmed"; 
    }

}

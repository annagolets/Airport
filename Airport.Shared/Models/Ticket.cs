using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Shared.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid RaceId { get; set; }
        public Race Race { get; set; }
        public int NumberSeat { get; set; }
        public decimal Cost { get; set; }
        public Passenger Passenger { get; set; }
        public Guid PassengerId { get; set; }
    }
}

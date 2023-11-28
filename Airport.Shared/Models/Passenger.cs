using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Shared.Models
{
    public class Passenger
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string PassportData { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}

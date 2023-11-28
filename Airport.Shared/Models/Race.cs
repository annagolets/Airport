using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Shared.Models
{
    public class Race
    {
        public Guid Id { get; set; }
        public int NumberRace { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public Airplane Airplane { get; set; }
        public Guid AirplaneId { get; set; }
        public Crew Crew { get; set; }
        public Guid CrewId { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}

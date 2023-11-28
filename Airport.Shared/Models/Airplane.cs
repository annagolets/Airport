using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Shared.Models
{
    public class Airplane
    {
        public Guid Id { get; set; }
        public string Mark { get; set; }
        public double Capacity { get; set; }
        public TypeAirplane TypeAirplane { get; set; }
        public Guid TypeAirplaneId { get; set; }
        public string Specifications { get; set; }
        public DateTime DateLastRepair { get; set; }
        public IEnumerable<Race> Races { get; set; }
    }
}

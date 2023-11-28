using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Shared.Models
{
    public class Crew
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public IEnumerable<Executive> Executives { get; set; }
        public IEnumerable<Race> Races { get; set; }
    }
}

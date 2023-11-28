using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Shared.Models
{
    public class TypeAirplane
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Appointment { get; set; }
        public string Limit { get; set; }
    }
}

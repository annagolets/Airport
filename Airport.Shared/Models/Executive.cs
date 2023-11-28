using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Shared.Models
{
    public class Executive
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public JobTitle JobTitle { get; set; }
        public Crew Crew { get; set; }
        public Guid CrewId { get; set; }
    }
}

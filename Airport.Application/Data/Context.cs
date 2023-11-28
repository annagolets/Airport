using Airport.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Airport.Application.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Executive> Executives { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TypeAirplane> TypeAirplanes { get; set; }
    }
}

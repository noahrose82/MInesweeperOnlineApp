using Microsoft.EntityFrameworkCore;
using Milestone1App.Models;

namespace Milestone1App.Data
{
    /// <summary>
    /// Application database context. Manages access to the SQL Server database and provides a DbSet for Users.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructor: passes DbContextOptions up to the base DbContext.
        /// </summary>
        /// <param name="options">The configuration options for the database context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// A table of User entities in the database. Each User object in code maps to a row in this table.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
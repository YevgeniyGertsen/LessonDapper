using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace LessonDapper.Model
{
    public partial class SportContextEfCore : DbContext
    {
        public SportContextEfCore()
            : base("name=SportContextEfCore")
        {
        }

        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Sport> Sport { get; set; }
        public virtual DbSet<Team> Team { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

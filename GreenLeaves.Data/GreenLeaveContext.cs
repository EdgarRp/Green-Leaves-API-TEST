using GreenLeaves.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenLeaves.Data {
    public class GreenLeaveContext : IdentityDbContext<UserModel> {
        public GreenLeaveContext(DbContextOptions<GreenLeaveContext> options) : base (options){

        }
        protected override void OnModelCreating( ModelBuilder builder ) {
            #region Create Tables
            builder.Entity<City>().ToTable( "Cities" ).Property(e => e.Id).ValueGeneratedOnAdd();
            base.OnModelCreating( builder );
            #endregion
        }

        public DbSet<UserModel> UserModel { get; set; }
        public DbSet<City> City { get; set; }
    }
}

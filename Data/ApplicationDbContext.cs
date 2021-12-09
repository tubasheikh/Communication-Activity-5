using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Importing Entity Framework (you have to use the nu-get package manager here).
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//Importing from the models folder.
using Lab5_1_.Models;

namespace Lab5_1_.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        //Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //You can make customizations here.
        }
    }
}

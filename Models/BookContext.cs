using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// import Microsoft.EntityFrameworkCore 
// make sure it is installed in the nuget packages
using Microsoft.EntityFrameworkCore;
namespace Lab5_1_.Models
{
    public class BookContext :DbContext
    {
        // it is a bridge between the entitity classes and the database 
        public BookContext(DbContextOptions<BookContext> options): base(options)
        {

        }

        //DbSet corresponds to a tables in the database
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<ContactAuthor> ContactAuthors { get; set; }
    }
}

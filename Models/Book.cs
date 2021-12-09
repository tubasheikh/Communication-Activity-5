using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5_1_.Models
{
    public class Book
    {
        // BookId is the primary key for this model 
        public int BookId { get; set; }
        public string Title { get; set; }

        // Each book will be associated to an author 
        // however an author, however could be associated to multiple books
        // one-to-many relationship
        public int AuthorId { get; set; }

        // Navigation property 
        public Author Author { get; set; }

        
    }
}

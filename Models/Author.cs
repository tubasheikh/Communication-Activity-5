using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5_1_.Models
{
    public class Author
    {
        // EF will recognize that AuthorId is the primary key 
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // This model contains a list of books 
        // it imlplies that one Author object and have a list of books associated with it
        // (one-to-many relationship)
        public List<Book> Books { get; set; }

        // Each author object/record will also be associated to a ContactAuthor object/record
        // this means they have a one-to-one relationship 
        public ContactAuthor Contact { get; set; }
    }
}

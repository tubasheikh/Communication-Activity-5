using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5_1_.Models
{
    public class ContactAuthor
    {
        // Primary key for AuthorContacts 
        public int ContactAuthorId { get; set; }

        public string phoneNumber { get; set; }

        public string city { get; set; }

        public string address { get; set; }

        // Each contacct will be associated to one author 
        // one-to-one relationship
        public int AuthorId { get; set; }

        // Navigation property 
        public Author Author { get; set; }
    }
}

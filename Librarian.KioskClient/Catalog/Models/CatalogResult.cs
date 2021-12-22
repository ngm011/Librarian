using System;

namespace Librarian.KioskClient.Catalog.Models
{
    public class CatalogResult 
    { 
        public DateTime CreatedOn { get; set; }
        public BookInfo[] Infos { get; set; }
    }
}

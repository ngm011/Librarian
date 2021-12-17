using System;

namespace Librarian.Services.Result
{
    public class CatalogResult 
    { 
        internal CatalogResult(BookInfo[] infos) 
        {
            CreatedOn = DateTime.Now;
            Infos = infos;
        }

        public DateTime CreatedOn { get; }
        public BookInfo[] Infos { get; }
    }
}

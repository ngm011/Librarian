using System;

namespace Librarian.Services
{
    [Serializable]
    public class CatalogServiceRetrievalException : Exception
    {
        public CatalogServiceRetrievalException(string message, Exception? innerException) : 
            base(message, innerException) { }
    }
}

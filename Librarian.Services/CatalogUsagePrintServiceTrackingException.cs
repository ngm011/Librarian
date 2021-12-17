using System;

namespace Librarian.Services
{
    [Serializable]
    public class CatalogUsagePrintServiceTrackingException : Exception
    {
        public CatalogUsagePrintServiceTrackingException(string message, Exception? innerException) :
            base(message, innerException)
        { }
    }
}

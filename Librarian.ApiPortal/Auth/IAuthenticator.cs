namespace Librarian.ApiPortal.Auth
{
    public interface IAuthenticator
    {
        string Authenticate(string apiKey);
    }
}

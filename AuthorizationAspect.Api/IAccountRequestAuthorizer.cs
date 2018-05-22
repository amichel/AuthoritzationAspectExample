namespace AuthorizationAspect.Api
{
    public interface IAccountRequestAuthorizer
    {
        bool Authorize(IAuthoriziedAccountRequest request);
    }
}

namespace AuthorizationAspect.Api
{
    public interface IAuthorizer
    {
        bool Authorize(object entity);
    }
}

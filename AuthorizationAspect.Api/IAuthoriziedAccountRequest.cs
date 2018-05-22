namespace AuthorizationAspect.Api
{
    public interface IAuthoriziedAccountRequest: IHasAccountNumber
    {
        string UserName { get; }
    }
}

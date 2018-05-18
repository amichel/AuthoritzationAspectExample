namespace AuthoritzationAspectExample
{
    public interface IAuthoriziedAccountRequest: IHasAccountNumber
    {
        string UserName { get; }
    }
}

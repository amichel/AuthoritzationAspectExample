namespace AuthoritzationAspectExample
{
    public interface IAuthoriziedAccountRequest
    {
        string UserName { get; }
        int AccountNumber { get; }
    }
}

namespace AuthoritzationAspectExample
{
    public interface IAuthoriziedEntityCommand<T> : IAuthoriziedAccountRequest
    {
        T EntityValidationKey { get; }
    }
}

namespace AuthoritzationAspectExample
{
    public interface IEntityCommandAuthorizer<TCommand, K> where TCommand : IAuthoriziedEntityCommand<K>
    {
        bool Authorize(TCommand command);
    }
}

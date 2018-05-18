namespace AuthoritzationAspectExample
{
    public interface IAuthorizer
    {
        bool Authorize(object entity);
    }

    public interface IEntityCommandAuthorizer<in TCommand> : IAuthorizer
        where TCommand : IAuthoriziedEntityCommand
    {
        bool Authorize(TCommand command);
    }
}

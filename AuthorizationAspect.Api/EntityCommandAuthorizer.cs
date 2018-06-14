namespace AuthorizationAspect.Api
{
    public abstract class EntityCommandAuthorizer<TCommand> : IAuthorizer
        where TCommand : IAuthoriziedEntityCommand
    {
        public EntityCommandAuthorizer()
        {
            
        }

        public virtual bool Authorize(object entity)
        {
            return Authorize((TCommand)entity);
        }

        public abstract bool Authorize(TCommand command);
    }
}
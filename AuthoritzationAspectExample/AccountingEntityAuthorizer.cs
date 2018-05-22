using AuthorizationAspect.Api;

namespace AuthorizationAspectExample
{
    class AccountingEntityAuthorizer : IEntityCommandAuthorizer<BaseAccountingRecordCommand>
    {
        public bool Authorize(BaseAccountingRecordCommand command)
        {
            //TODO: return AuthorizationProvider.Accounting.BelongsTo(command.EntityValidationKey,command.AccountNumber);
            return command.RecordId % 2 == 0; //something for mock
        }

        public bool Authorize(object entity)
        {
            return Authorize(entity as BaseAccountingRecordCommand);
        }
    }
}

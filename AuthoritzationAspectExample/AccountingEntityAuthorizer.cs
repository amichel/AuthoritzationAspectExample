using AuthorizationAspect.Api;

namespace AuthorizationAspectExample
{
    class AccountingEntityAuthorizer : EntityCommandAuthorizer<BaseAccountingRecordCommand>
    {
        public override bool Authorize(BaseAccountingRecordCommand command)
        {
            //TODO: return AuthorizationProvider.Accounting.BelongsTo(command.EntityValidationKey,command.AccountNumber);
            return command.RecordId % 2 == 0; //something for mock
        }
    }
}

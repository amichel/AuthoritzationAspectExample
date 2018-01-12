namespace AuthoritzationAspectExample
{
    class AccountingEntityAuthorizer : IEntityCommandAuthorizer<BaseAccountingRecordCommand, long>
    {
        public bool Authorize(BaseAccountingRecordCommand command)
        {
            //TODO: return AuthorizationProvider.Accounting.BelongsTo(command.EntityValidationKey,command.AccountNumber);
            return command.EntityValidationKey % 2 == 0; //something for mock
        }
    }
}

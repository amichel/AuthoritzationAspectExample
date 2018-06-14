using System.Collections.Generic;

namespace AuthoritzationAspect.Tests.AccountingService
{
    public interface IAccountingService
    {
        IList<AccountRecord> GetAccountStatement(AccountStatementRequest request);
        IList<AccountRecord> CancelAccountRecord(CancelAccountRecordRequest request);
    }
}

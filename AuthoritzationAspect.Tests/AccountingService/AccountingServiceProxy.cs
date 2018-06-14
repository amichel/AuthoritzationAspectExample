using System;
using System.Collections.Generic;
using AuthorizationAspect.Tests;

namespace AuthoritzationAspect.Tests.AccountingService
{
    [AuthorizationAspect.Api.AuthorizationAspect]
    public class AccountingServiceProxy : IAccountingService
    {
        private readonly IAccountingService _source;

        public AccountingServiceProxy(IAccountingService source)
        {
            _source = source;
        }

        public IList<AccountRecord> GetAccountStatement(AccountStatementRequest request)
        {
            return _source.GetAccountStatement(request);
        }
        public IList<AccountRecord> CancelAccountRecord(CancelAccountRecordRequest request)
        {
            return _source.CancelAccountRecord(request);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoritzationAspectExample
{
    [AuthorizationAspect]
    class AccountingService : IAccountingService
    {
        public IList<AccountRecord> GetAccountStatement(AccountStatementRequest request)
        {
            return new List<AccountRecord>();
        }
        public IList<AccountRecord> CancelAccountRecord(CancelAccountRecordRequest request)
        {
            return new List<AccountRecord>();
        }
    }
}

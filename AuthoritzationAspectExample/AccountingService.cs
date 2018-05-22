using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAspectExample
{
    [AuthorizationAspect.Api.AuthorizationAspect]
    public class AccountingService : IAccountingService
    {
        public IList<AccountRecord> GetAccountStatement(AccountStatementRequest request)
        {
            return new List<AccountRecord>
            {
                new AccountRecord{AccountNumber = request.AccountNumber, Balance = 1500, Timestamp = DateTime.UtcNow},
                new AccountRecord{AccountNumber = 999,Balance=0, Timestamp = DateTime.UtcNow}
            };
        }
        public IList<AccountRecord> CancelAccountRecord(CancelAccountRecordRequest request)
        {
            return new List<AccountRecord>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAspectExample
{
    public interface IAccountingService
    {
        IList<AccountRecord> GetAccountStatement(AccountStatementRequest request);
        IList<AccountRecord> CancelAccountRecord(CancelAccountRecordRequest request);
    }
}

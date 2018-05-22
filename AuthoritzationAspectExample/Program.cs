using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAspectExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new AccountingService();
            service.GetAccountStatement(new AccountStatementRequest() { AccountNumber = 12, UserName = "12" });

            var records = service.CancelAccountRecord(
                new CancelAccountRecordRequest() { AccountNumber = 12, CancellationReason = 1, UserName = "12" });
        }
    }
}

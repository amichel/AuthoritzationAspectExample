using AuthorizationAspect.Api;
using System;

namespace AuthoritzationAspect.Tests.AccountingService
{
    public class AccountRecord : IHasAccountNumber
    {
        public long RecordId { get; set; }
        public int AccountNumber { get; set; }
        public int Balance { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public abstract class BaseAccountingRequest : IAuthoriziedAccountRequest
    {
        public int AccountNumber { get; set; }
        public string UserName { get; set; }
    }

    public abstract class BaseAccountingRecordCommand : BaseAccountingRequest, IAuthoriziedEntityCommand
    {
        public long RecordId { get; set; }
    }

    public class AccountStatementRequest : BaseAccountingRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class CancelAccountRecordRequest : BaseAccountingRecordCommand
    {
        public int CancellationReason { get; set; }
    }
}

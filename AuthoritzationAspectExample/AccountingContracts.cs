﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizationAspect.Api;

namespace AuthorizationAspectExample
{
    public class AccountRecord : IHasAccountNumber
    {
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
        public long RecordId { get; }
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using AuthoritzationAspect.Tests.AccountingService;
using AuthorizationAspect.Api;
using AuthorizationAspect.Tests;
using NSubstitute;
using Xunit;

namespace AuthoritzationAspect.Tests
{
    public static class MockData
    {
        public static IList<AccountRecord> Records = new List<AccountRecord>
        {
            new AccountRecord {RecordId  = 10, AccountNumber = 1, Balance = 100, Timestamp = new DateTime(2018, 5, 13, 20, 0, 0) },
            new AccountRecord {RecordId  = 20, AccountNumber = 2, Balance = 200, Timestamp = new DateTime(2018, 5, 13, 21, 0, 0) },
            new AccountRecord {RecordId  = 30, AccountNumber = 3, Balance = -100, Timestamp = new DateTime(2018, 5 ,13, 22, 0, 0) }
        };
    }

    public class AuthorizerMock : EntityCommandAuthorizer<BaseAccountingRecordCommand>, IAccountRequestAuthorizer
    {
        public bool Authorize(IAuthoriziedAccountRequest request)
        {
            return string.Compare(request.UserName, request.AccountNumber.ToString(), true,
                       CultureInfo.InvariantCulture) == 0 || request.UserName == "superuser";
        }

        public override bool Authorize(BaseAccountingRecordCommand command)
        {
            return MockData.Records.FirstOrDefault(x =>
                       x.RecordId == command.RecordId && x.AccountNumber == command.AccountNumber) != null;
        }
    }

    public class AccountingServiceTest
    {
        private readonly Lazy<IAccountingService> _service = new Lazy<IAccountingService>(() =>
        {
            new ExecutingAssemblyIocContainer().Build();

            var mock = Substitute.For<IAccountingService>();
            mock.GetAccountStatement(Arg.Any<AccountStatementRequest>())
                .Returns(x => x.Arg<AccountStatementRequest>().UserName == "-1" ? MockData.Records : //TODO: for some reason it didn't work with NSubst pattern matching in args
                MockData.Records.Where(r => r.AccountNumber == x.Arg<AccountStatementRequest>().AccountNumber).ToList());

            mock.CancelAccountRecord(Arg.Any<CancelAccountRecordRequest>())
                .Returns(x => MockData.Records.Where(r => r.RecordId == x.Arg<CancelAccountRecordRequest>().RecordId).ToList());

            return new AccountingServiceProxy(mock);
        });


        [Fact]
        public void Success_GetAccountBalance_ForEligibleUserAndValidRequest()
        {
            var result = _service.Value.GetAccountStatement(new AccountStatementRequest
            {
                AccountNumber = 1,
                UserName = "1"
            });

            Assert.NotEmpty(result);
        }

        [Fact]
        public void SecurityException_GetAccountBalance_ForUnauthorizedUser()
        {
            Assert.Throws<SecurityException>(() => _service.Value.GetAccountStatement(new AccountStatementRequest
            {
                AccountNumber = 1,
                UserName = "2"
            }));
        }

        [Fact]
        public void SecurityException_GetAccountBalance_WhenRecordsOfOtherUsersInResponse()
        {
            Assert.Throws<SecurityException>(() => _service.Value.GetAccountStatement(new AccountStatementRequest
            {
                AccountNumber = -1,
                UserName = "-1"
            }));
        }

        [Fact]
        public void Success_CancelAccountRecord_WhenRecordBelongsToSameAccount()
        {
            var result = _service.Value.CancelAccountRecord(new CancelAccountRecordRequest
            {
                AccountNumber = 1,
                UserName = "1",
                RecordId = 10,
                CancellationReason = 1
            });

            Assert.NotEmpty(result);
        }

        [Fact]
        public void SecurityException_CancelAccountRecord_WhenRecordBelongsToOtherAccount()
        {
            Assert.Throws<SecurityException>(() => _service.Value.CancelAccountRecord(new CancelAccountRecordRequest
            {
                AccountNumber = 1,
                UserName = "1",
                RecordId = 20,
                CancellationReason = 1
            }));
        }
    }
}

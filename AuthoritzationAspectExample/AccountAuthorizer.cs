using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoritzationAspectExample
{
    class AccountAuthorizer : IAccountRequestAuthorizer
    {
        public bool Authorize(IAuthoriziedAccountRequest request)
        {
            //TODO: return AuthorizationProvider.Users.HasPermissions(request.UserName,request.AccountNumber);
            return request.AccountNumber.ToString() == request.UserName; //something for mock
        }
    }
}

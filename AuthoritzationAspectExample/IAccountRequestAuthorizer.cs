using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoritzationAspectExample
{
    public interface IAccountRequestAuthorizer
    {
        bool Authorize(IAuthoriziedAccountRequest request);
    }
}

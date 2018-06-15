using System;

namespace AuthorizationAspect.Api
{
    [Flags]
    public enum AuthorizationMode
    {
        None = 0b0000,
        AccountEntityCommand = 0b0001,
        AccountRequest = 0b0010,
        ReturnValue = 0b0110,
        All = 0b0111
    }
}
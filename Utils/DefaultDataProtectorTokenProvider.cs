using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace TripickServer.Utils
{
    public class DefaultDataProtectorTokenProvider<T> : DataProtectorTokenProvider<T> where T : class
    {
        public DefaultDataProtectorTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<DefaultDataProtectorTokenProviderOptions> options)
        : base(dataProtectionProvider, options, null)
        {
        }
    }
}

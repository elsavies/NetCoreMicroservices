using System.Collections.Generic;

namespace Auth_API.Helpers
{
    public class AppSettings
    {
        public JwtParameters JwtParameters { get; set; }
    }

    public class JwtParameters
    {
        public IEnumerable<string> JwtIssuers { get; set; }
        public IEnumerable<string> JwtAudiences { get; set; }
        public string JwtKey { get; set; }
        public long JwtExpiryInHours { get; set; }
    }
}

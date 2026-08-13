using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Settings
{
    public class JwtSettings
    {
        public const string Section = "Jwt";

        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public float ExpireMinutes { get; set; }
        public float RefreshTokenExpireDays { get; set; }
    }
}

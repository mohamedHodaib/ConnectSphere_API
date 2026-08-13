using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Settings
{
    public class AdminInfo
    {
        public const string Section = "Admin";

        public string Email { get; set; } = null!;
        public string Password {get; set;} = null!;
        public string EmailConfirmed { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}

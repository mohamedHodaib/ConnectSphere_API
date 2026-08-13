using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Settings
{
    public class ValidationSettings
    {
        public const string Section = "ValidationSettings";

        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}

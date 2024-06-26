using System;
using System.Collections.Generic;
using System.Text;

namespace Internships.Core.Settings
{
    public class TokenSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInDays { get; set; }
        public string FrontendUrl { get; set; }
    }
}

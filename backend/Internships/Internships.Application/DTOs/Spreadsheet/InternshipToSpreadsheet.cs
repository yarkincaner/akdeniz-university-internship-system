using System;
using System.Collections.Generic;
using System.Text;

namespace Internships.Core.DTOs.Spreadsheet
{
    public class InternshipToSpreadsheet
    {
        public long TcKimlikNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
    }
}

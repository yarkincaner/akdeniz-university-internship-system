using System;
using System.Collections.Generic;

namespace Internships.Core.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int DepartmentID { get; set; }
        public bool IsEnabled { get; set; }

        public long TcKimlikNo { get; set; }
        public int BirthYear { get; set; }

        public Department Departmant { get; set; }

        public ICollection<Internship> Internships { get; set; }

        public UserRoles UserRoles { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
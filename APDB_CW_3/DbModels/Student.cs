﻿using System;
using System.Collections.Generic;

namespace APDB_CW_3.DbModels
{
    public partial class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual Enrollment IdEnrollmentNavigation { get; set; }
    }
}

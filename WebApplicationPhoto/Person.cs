using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationPhoto
{
    public class Person
    {
        public int PersonID { get; set; }
        public string NationalInsuranceNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Photo { get; set; }
    }

}
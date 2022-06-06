using System;
using System.Collections.Generic;

namespace ProcessFunction.Models
{
    public partial class PersonInfo
    {
        public int BusinessEntityId { get; set; }
        public string PersonType { get; set; }
        public int NameStyle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmailPromotion { get; set; }
    }
}

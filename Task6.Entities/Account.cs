using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task6.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Permissions { get; set; }
    }
}
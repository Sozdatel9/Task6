using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task6.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string Award { get; set; }

        public int AwardImgId { get; set; }

        public int ImageId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CampaignService.Models
{

    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SSN { get; set; }
        public DateOnly? DOB { get; set; }
        public Address Home { get; set; }
        public Address Office { get; set; }
        public string[] Colors { get; set; }
        public int Age
        {
            get
            {
                if (DOB == null)
                {
                    return 0;
                }

                var today = DateOnly.FromDateTime(DateTime.Now);
                int age = today.Year - DOB.Value.Year;

                if (today < DOB.Value.AddYears(age))
                {
                    age--;
                }

                return age;
            }
        }
    }
}

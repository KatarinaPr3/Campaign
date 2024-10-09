using System.Xml.Linq;

namespace CampaignService.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public static Address ParseAddress(XElement addressElement)
        {
            if (addressElement == null) return null;

            var address = new Address
            {
                Street = addressElement.Element("Street")?.Value,
                City = addressElement.Element("City")?.Value,
                State = addressElement.Element("State")?.Value,
                Zip = addressElement.Element("ZipCode")?.Value
            };
            return address;

        }
    }
}

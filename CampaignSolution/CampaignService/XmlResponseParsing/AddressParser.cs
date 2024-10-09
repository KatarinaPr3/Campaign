using CampaignService.Enums;
using CampaignService.Interfaces;
using CampaignService.Models;
using System.Xml.Linq;

namespace CampaignService.XmlResponseParsing
{
    public class AddressParser : IAddressParser
    {
        public Address GetAddress(XElement element, XNamespace ns, EnumsResponses addressType)
        {
            try
            {
                return new Address
                {
                    Street = GlobalParser.GetElementValue(element.Descendants(ns + addressType.ToString()).FirstOrDefault(), ns, EnumsResponses.Street),
                    City = GlobalParser.GetElementValue(element.Descendants(ns + addressType.ToString()).FirstOrDefault(), ns, EnumsResponses.City),
                    State = GlobalParser.GetElementValue(element.Descendants(ns + addressType.ToString()).FirstOrDefault(), ns, EnumsResponses.State),
                    Zip = GlobalParser.GetElementValue(element.Descendants(ns + addressType.ToString()).FirstOrDefault(), ns, EnumsResponses.Zip),
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}");
            }
        }
    }
}

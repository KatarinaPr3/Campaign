using CampaignService.Enums;
using CampaignService.Models;
using System.Xml.Linq;

namespace CampaignService.Interfaces
{
    public interface IAddressParser
    {
        Address GetAddress(XElement element, XNamespace ns, EnumsResponses addressType);

    }
}

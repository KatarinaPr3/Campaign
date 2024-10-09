using CampaignService.Enums;
using CampaignService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CampaignService.Interfaces
{
    public interface IAddressParser
    {
        Address GetAddress(XElement element, XNamespace ns, EnumsResponses addressType);

    }
}

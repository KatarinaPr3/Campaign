using CampaignService.Enums;
using System.Xml.Linq;

namespace CampaignService.XmlResponseParsing
{
    public class GlobalParser
    {
        public static string GetElementValue(XElement element, XNamespace ns, EnumsResponses response)
        {
            try
            {
                return element.Element(ns + $"{response.ToString()}")?.Value;

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}");
            }
        }
    }
}

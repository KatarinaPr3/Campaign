using CampaignService.Constants;
using System.Xml.Linq;

namespace CampaignService.XmlResponseParsing
{
    public class ColorsParser
    {
        public static string[]? ParseColors(XElement favoriteColorsElement)
        {
            var colors = new List<string>();

            if (favoriteColorsElement != null)
            {
                XNamespace ns = XmlConstants.XNAMESPACE_NS;

                colors.AddRange(favoriteColorsElement.Elements(ns + "FavoriteColorsItem").Select(color => color.Value));
            }

            return colors.ToArray();
        }
    }
}

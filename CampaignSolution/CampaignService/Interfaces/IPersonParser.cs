using CampaignService.Models;
using System.Xml.Linq;

namespace CampaignService.Interfaces
{
    public interface IPersonParser
    {
        /// <summary>
        /// Parses a list of customers from the provided XML response string.
        /// </summary>
        /// <param name="responseXml">The XML response string.</param>
        /// <returns>A list of Person objects parsed from the response.</returns>
        List<Person> ParseCustomerFromResponseForList(string responseXml);

        /// <summary>
        /// Validates the customer element in the provided XML document.
        /// </summary>
        /// <param name="doc">The XML document to validate.</param>
        /// <param name="ns">The XML namespace to use for parsing.</param>
        /// <returns>The validated customer XElement.</returns>
        XElement ValidateCustomerElement(XDocument doc, XNamespace ns);

        /// <summary>
        /// Parses a person of a specific type from the provided XML response string.
        /// </summary>
        /// <typeparam name="T">The type of person to parse (e.g., Employee or Customer).</typeparam>
        /// <param name="responseXml">The XML response string.</param>
        /// <param name="id">Optional ID for the person being parsed.</param>
        /// <returns>The parsed person object of type T.</returns>
        T ParseTypeFromResponse<T>(string responseXml, int? id = null) where T : Person, new();
    }
}

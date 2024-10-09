using CampaignService.Constants;
using CampaignService.Enums;
using CampaignService.Models;
using System.Xml.Linq;
using CampaignService.Interfaces;

namespace CampaignService.XmlResponseParsing
{
    public class PersonParser : IPersonParser
    {
        private readonly IAddressParser _addressParser;
        public PersonParser(IAddressParser addressParser)
        {
            _addressParser = addressParser;
        }
        public List<Person> ParseCustomerFromResponseForList(string responseXml)
        {
            XDocument doc = XDocument.Parse(responseXml);
            XNamespace ns = XmlConstants.XNAMESPACE_NS;

            List<Person> persons = doc.Descendants(ns + $"{EnumsResponses.PersonIdentification.ToString()}")
            .Select(personElement => new Person
            {
                ID = int.TryParse(GlobalParser.GetElementValue(personElement, ns, EnumsResponses.ID), out int id) ? id : 0,
                Name = GlobalParser.GetElementValue(personElement, ns, EnumsResponses.Name),
                SSN = GlobalParser.GetElementValue(personElement, ns, EnumsResponses.SSN),
                DOB = DateOnly.TryParse(GlobalParser.GetElementValue(personElement, ns, EnumsResponses.DOB), out var dob) ? dob : DateOnly.MinValue,
            }).ToList();

            return persons;
        }

        public T ParseTypeFromResponse<T>(string responseXml, int? id = null) where T : Person, new()
        {

            try
            {
                XDocument doc = XDocument.Parse(responseXml);
                XNamespace ns = $"{XmlConstants.XNAMESPACE_NS}";
                XElement personElement = ValidateCustomerElement(doc, ns);

                var xsiNamespace = XmlConstants.XSINAMESPACE;
                var personType = personElement.Attribute(XNamespace.Get(xsiNamespace) + $"{EnumsResponses.type.ToString()}")?.Value.Split(':')[0];

                if (personType != null && typeof(T) == typeof(Employee))
                {
                    return (T)(object)new Employee
                    {
                        ID = id ?? 0,
                        Name = GlobalParser.GetElementValue(personElement, ns, EnumsResponses.Name),
                        SSN = GlobalParser.GetElementValue(personElement, ns, EnumsResponses.SSN),
                        DOB = DateOnly.TryParse(GlobalParser.GetElementValue(personElement, ns, EnumsResponses.DOB), out var dob) ? dob : DateOnly.MinValue,
                        Home = _addressParser.GetAddress(personElement, ns, EnumsResponses.Home),
                        Office = _addressParser.GetAddress(personElement, ns, EnumsResponses.Office),
                        Colors = ColorsParser.ParseColors(personElement.Descendants(ns + $"{EnumsResponses.FavoriteColors}").FirstOrDefault()),
                        Title = GlobalParser.GetElementValue(personElement, ns, EnumsResponses.Title),
                        Salary = decimal.Parse(GlobalParser.GetElementValue(personElement, ns, EnumsResponses.Salary)),
                        Spouse = ParseTypeFromResponse<Customer>(personElement.Element(ns + $"{EnumsResponses.Spouse.ToString()}")?.ToString(), null),
                    };
                }
                else if (personType is null && typeof(T) == typeof(Customer))
                {
                    return (T)(object)new Customer
                    {
                        ID = id ?? 0,
                        Name = GlobalParser.GetElementValue(personElement, ns, EnumsResponses.Name),
                        SSN = GlobalParser.GetElementValue(personElement, ns, EnumsResponses.SSN),
                        DOB = DateOnly.TryParse(GlobalParser.GetElementValue(personElement, ns, EnumsResponses.DOB), out var dob) ? dob : DateOnly.MinValue,
                        Home = _addressParser.GetAddress(personElement, ns, EnumsResponses.Home),
                        Office = _addressParser.GetAddress(personElement, ns, EnumsResponses.Office),
                        Colors = ColorsParser.ParseColors(personElement.Descendants(ns + $"{EnumsResponses.FavoriteColors}").FirstOrDefault()),
                    };
                }
                else if (personType is not null && typeof(T) == typeof(Customer))
                {
                    throw new Exception($"You requested data for a customer, but the ID you passed is not within the valid range for customer IDs. The values are defined in XmlConstants.");


                }
                else if (personType is null && typeof(T) == typeof(Employee))
                {
                    throw new Exception($"You requested data for an employee, but the ID you passed is not within the valid range for employee IDs. The values are defined in XmlConstants.");
                }
                else
                {
                    throw new Exception($"Unsupported type: {typeof(T).Name}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while parsing the response: " + ex.Message);
            }
        }

        public XElement ValidateCustomerElement(XDocument doc, XNamespace ns)
        {
            XElement customerElement = GetElement(doc, ns, EnumsResponses.FindPersonResult)
                                                   ?? GetElement(doc, ns, EnumsResponses.Spouse);

            if (customerElement is null)
            {
                throw new Exception("The requested element was not found in the XML response.");
            }
            return customerElement;
        }

        private static XElement GetElement(XDocument doc, XNamespace ns, EnumsResponses element)
        {
            return doc.Descendants(ns + element.ToString()).FirstOrDefault();
        }
    }
}

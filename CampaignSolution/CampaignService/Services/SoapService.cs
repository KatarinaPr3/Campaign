using CampaignService.Interfaces;
using CampaignService.Models;

namespace CampaignService.Services
{
    public class SoapService : ISoapService
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly IPersonParser _personParser;

        public SoapService(IPersonParser personParser)
        {
            _personParser = personParser;

        }

        public async Task<T> FindPersonById<T>(int id) where T : Person, new()
        {
            try
            {
                var url = $"https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson&id={id}";

                HttpResponseMessage response = await _client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var customer = _personParser.ParseTypeFromResponse<T>(result, id);
                    return customer;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}");
            }
        }

        public async Task<List<Person>> GetAllCustomers()
        {
            List<Person> customers = new();

            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                var url = $"https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=GetListByName&name={letter}";

                HttpResponseMessage response = await _client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var parsed_customers = _personParser.ParseCustomerFromResponseForList(result);
                    foreach (var item in parsed_customers)
                    {
                        customers.Add(item);
                    }
                }
            }

            return customers;
        }
    }

}

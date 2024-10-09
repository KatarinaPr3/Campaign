using CampaignService.Constants;
using CampaignService.Helpers;
using CampaignService.Interfaces;
using CampaignService.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        public async Task<List<Agent>> CreateAgentsFromEmployees()
        {
            List<Employee> employees = await GetAllEmployeesRoleAgent();
            List<Agent> agents = new();
            foreach (Employee employee in employees)
            {
                agents.Add(UsernameHelper.GenerateAgentWithUsernamePassword(employee));
            }
            return agents;
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

        public async Task<List<Person>> GetPeople()
        {
            List<Person> people = new();

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
                        people.Add(item);
                    }
                }
            }

            return people;
        }

        public async Task<List<Employee>> GetAllEmployeesRoleAgent()
        {
            List<Employee> employees = new();
            for (int i = Constants.Settings.EMPLOYEE_ID_MIN; i <= Constants.Settings.EMPLOYEE_ID_MAX; i++)
            {
                var employee = await FindPersonById<Employee>(i);

                if (employee != null)
                {
                    if (!Constants.XmlConstants.NON_AGENT_JOB_TITLES.Contains(employee.Title)) employees.Add(employee);

                }
            }
            return employees;
        }

        public async Task<List<Person>> GetAllCustomers()
        {
            List<Person> customers = new();
            for (int i = Settings.CUSTOMER_ID_MIN; i < Settings.CUSTOMER_ID_MAX; i++)
            {
                var customer = await FindPersonById<Customer>(i);
                customers.Add(customer);

            }
            return customers.Any() ? customers : null;

        }


    }

}

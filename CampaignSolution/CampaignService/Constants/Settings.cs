using Microsoft.AspNetCore.Http;

namespace CampaignService.Constants
{
    public class Settings
    {
        public const int CUSTOMER_ID_MIN = 1;
        public const int CUSTOMER_ID_MAX = 100;

        public const int EMPLOYEE_ID_MIN = 101;
        public const int EMPLOYEE_ID_MAX = 200;

        public const string AGENT_PASSWORD = "abcd1234";
        public const string CUSTOMER_PASSWORD = "abcd1234";

        public const string MESSAGE_STATUS_INTERNAL_SERVER_ERROR = "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.";

    }
}

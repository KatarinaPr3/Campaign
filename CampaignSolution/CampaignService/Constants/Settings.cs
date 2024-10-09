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

        public const int AGENT_CAMPAIGN_LIMIT_MAX = 5;
        public const int GIVING_REWARDS_DURATION_DAYS = 7;
        public static readonly DateTime GIVING_REWARDS_START_DATE = new DateTime(2024, 07, 9);
    }
}

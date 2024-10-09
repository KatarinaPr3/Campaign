namespace CampaignService.Models
{
    public class Customer : Person
    {
        public bool LoyalCustomer
        {
            get
            {
                if (Colors != null && Colors.Length > 0) return true;
                else return false;
            }
        }
    }
}

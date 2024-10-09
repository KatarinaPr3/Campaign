namespace CampaignService.Models
{
    public class Employee : Person
    {
        public Person Spouse { get; set; }
        public string Title { get; set; }
        public decimal Salary { get; set; }
    }
}

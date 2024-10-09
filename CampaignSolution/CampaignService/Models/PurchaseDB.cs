namespace CampaignService.Models
{
    public class PurchaseDB
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public double Discount { get; set; }
        public decimal Total { get; set; }
        public ICollection<PurchasedProduct> PurchasedProducts { get; set; }

        public void CalculateTotal()
        {
            decimal total = PurchasedProducts.Sum(item => item.Product.Price * item.Quantity);
            decimal discountAmount = total * (decimal)(Discount / 100);
            Total = total - discountAmount;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignService.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<ProductDB> Products { get; set; } = new();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public double Discount { get; set; }
        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var item in Products)
                {
                    total += item.Price;
                }
                decimal discountAmount = (total * (decimal)(Discount / 100));

                total -= discountAmount;
                return total;
            }
        }
    }
}

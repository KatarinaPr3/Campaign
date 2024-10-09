using System.Text.Json.Serialization;

namespace CampaignService.Models
{
    public class PurchasedProduct
    {
        public int PurchasedProductId { get; set; }

        public int ProductId { get; set; }
        [JsonIgnore]
        public ProductDB Product { get; set; }
        public int PurchaseDBId { get; set; }
        [JsonIgnore]
        public PurchaseDB PurchaseDB { get; set; }

        public int Quantity { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

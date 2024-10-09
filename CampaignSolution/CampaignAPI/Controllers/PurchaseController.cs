using CampaignAPI.DB.Interfaces;
using CampaignService.Enums;
using CampaignService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace CampaignAPI.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IEntityService<PurchaseDB> _entityService;
        private readonly IEntityService<ProductDB> _entityServiceProduct;
        private readonly IEntityService<Reward> _rewardService;
        private readonly IEntityService<Campaign> _campaignService;
        private readonly ICampaignService _customCampaignService;

        public PurchaseController(IEntityService<PurchaseDB> entityService, IEntityService<ProductDB> entityServiceProduct, IEntityService<Reward> rewardService, IEntityService<Campaign> campaignService, ICampaignService customCampaignService)
        {
            _rewardService = rewardService;
            _campaignService = campaignService;
            _customCampaignService = customCampaignService;
            _entityService = entityService;
            _entityServiceProduct = entityServiceProduct;
        }

        [HttpPost("add_purchase", Name = "MakePurchase")]
        [ProducesResponseType(typeof(PurchaseDB), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = nameof(Roles.User))]
        public async Task<ActionResult<PurchaseDB>> MakePurchase([FromBody] PurchaseAPI purchase)
        {
            int loggedUserId = 0;
            if (purchase == null || purchase.Products == null || !purchase.Products.Any())
            {
                return BadRequest(new { message = "Purchase data is required." });
            }

            var nameIdentifierClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (nameIdentifierClaim != null && int.TryParse(nameIdentifierClaim.Value, out loggedUserId))
            {
                var allProductsFromDB = await _entityServiceProduct.GetAllAsync();

                var missingProducts = purchase.Products
                    .Where(p => !allProductsFromDB.Any(dbProduct => dbProduct.Id == p.Id))
                    .ToList();

                if (missingProducts.Any())
                {
                    return NotFound(new
                    {
                        message = "You attempted to add products with IDs that are not recorded in the database.",
                        missingProducts
                    });
                }

                var products = await ProductsDbBFromProductsAPI(purchase.Products);
                var discount = await GetDiscountForCustomerAsync(loggedUserId);

                var newPurchase = new PurchaseDB
                {
                    CustomerId = loggedUserId,
                    DateTime = DateTime.Now,
                    PurchasedProducts = products,
                    Discount = discount,
                };

                newPurchase.CalculateTotal();
                await _entityService.AddAsync(newPurchase);
                if (discount > 0)
                {
                    var rewards = await _rewardService.GetAllAsync();
                    var userRewards = rewards.Where(_ => _.CustomerId == loggedUserId).ToList();
                    foreach (Reward reward in userRewards) 
                    {
                        reward.Used = true;
                        await _rewardService.UpdateAsync(reward);
                    }
                }
                return Ok(newPurchase);
            }
            return BadRequest("Undefined purchase customer id");
        }


        private async Task<List<PurchasedProduct>> ProductsDbBFromProductsAPI(IEnumerable<ProductAPI> productAPIs)
        {
            var productList = new List<PurchasedProduct>();
            foreach (var item in productAPIs)
            {
                var product = await _entityServiceProduct.GetByIdAsync(item.Id);

                PurchasedProduct purchasedProduct = new()
                {
                    ProductId = item.Id,
                    Product = product,
                    Quantity = 1,
                };
                productList.Add(purchasedProduct);
            }
            return productList;
        }

        private async Task<double> GetDiscountForCustomerAsync(int customerId)
        {
            IEnumerable<Reward> rewards = await _rewardService.GetAllAsync();
            bool customerHasReward = rewards.Where(_ => _.CustomerId == customerId && !_.Used).Any();
            if (customerHasReward)
            {
                var allCampaigns = await _campaignService.GetAllAsync();
                Campaign activeCampaign = _customCampaignService.GetFirstActiveCampaignAsync(allCampaigns.ToList());
                return _customCampaignService.GetCampaignDiscount(activeCampaign);
            }
            return 0;
        }
    }
}

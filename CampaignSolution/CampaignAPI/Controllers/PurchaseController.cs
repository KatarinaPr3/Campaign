using CampaignAPI.DB.Interfaces;
using CampaignAPI.DB.Service;
using CampaignService.Enums;
using CampaignService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CampaignAPI.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IEntityService<PurchaseDB> _entityService;
        private readonly IEntityService<ProductDB> _entityServiceProduct;

        public PurchaseController(IEntityService<PurchaseDB> entityService, IEntityService<ProductDB> entityServiceProduct)
        {
            _entityService = entityService;
            _entityServiceProduct = entityServiceProduct;
        }

        [HttpPost("add_purchase", Name = "MakePurchase")]
        [ProducesResponseType(typeof(PurchaseDB), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = nameof(Roles.User))]
        public async Task<ActionResult<PurchaseDB>> MakePurchase([FromBody] PurchaseAPI purchase)
        {
            if (purchase == null || purchase.Products == null || !purchase.Products.Any())
            {
                return BadRequest(new { message = "Purchase data is required." });
            }

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
            var discount = 0; // TODO: GetDiscountForCustomerAsync(98);

            var newPurchase = new PurchaseDB
            {
                CustomerId = 98,
                DateTime = DateTime.Now,
                PurchasedProducts = products,
                Discount = discount,
            };

            newPurchase.CalculateTotal();
            await _entityService.AddAsync(newPurchase);
            // TODO: if discount > 0 use reward service to mark that user used reward .CustomerUseReward(id customer)

            return Ok(newPurchase);
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
    }
}

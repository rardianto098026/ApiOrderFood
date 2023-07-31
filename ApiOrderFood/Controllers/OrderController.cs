using ApiOrderFood.Models;
using ApiOrderFood.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiOrderFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IConfiguration _configuration;
        public OrderController(IOrderServices orderServices, IConfiguration configuration)
        {
            _orderServices = orderServices;
            _configuration = configuration;
        }
        [Authorize]
        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu()
        {
            try
            {
                var data = await _orderServices.GetAllMenu();
                if (data.Count != 0)
                {
                    return Ok(data);
                }
                else
                {
                    return Ok("null");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("AddOrder")]
        public IActionResult AddOrder([FromBody] Order order)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string userId = "";
            var tokenHandler = new JwtSecurityTokenHandler();

            // Set the TokenValidationParameters with the correct signing key
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                //ValidAudience = "https://localhost:7161",
                //ValidIssuer = "https://localhost:7161",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
                ClockSkew = TimeSpan.Zero
            };

            // Read the token and parse it to a ClaimsPrincipal
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                // Find the "sub" claim, which typically contains the user ID
                if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
                {
                    var userIdClaim = claimsIdentity.Claims.FirstOrDefault();
                    if (userIdClaim != null)
                    {
                        userId = userIdClaim.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error or return null)
                Console.WriteLine("Token validation failed:", ex.Message);
            }

            // Insert the order into the database (tr_orders table)
            var newOrder = new Order();

            using (var dbContext = new DatabaseContext())
            {
                // Get the last order ID from the table
                string lastOrderId = dbContext.Order.DefaultIfEmpty()
                                  .Max(o => o != null ? o.OrderId : null);


                // Extract the numeric part of the last order ID and increment it
                if (int.TryParse(lastOrderId.Substring(3), out int lastIdNumericPart))
                {
                    int newIdNumericPart = lastIdNumericPart + 1;

                    // Create the new order ID by concatenating "ABC" with the new numeric part
                    string newOrderId = "ABC" + DateTime.Now.Date.ToString("ddMMyyyy") + newIdNumericPart.ToString("D3");

                    // Create the new order entity
                    newOrder = new Order
                    {
                        OrderId = newOrderId,
                        TableNumber = order.TableNumber,
                        CreatedBy = Convert.ToInt32(userId),
                        CreatedDate = DateTime.Now,
                        TotalAmount = 0 // You might want to set the total amount based on the order items later
                    };

                    // Add the new order to the context and save changes
                    dbContext.Order.Add(newOrder);
                    dbContext.SaveChanges();
                }
            }

            // Insert the order items into the database (tr_order_items table)
            using (var dbContext = new DatabaseContext())
            {
                foreach (var item in order.Items)
                {
                    var newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.OrderId,
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity
                        // You may need to set other properties as per your requirement
                    };
                    dbContext.OrderItem.Add(newOrderItem);
                }
                dbContext.SaveChanges();
            }

            // Return a successful response
            return Ok("Order submitted successfully");
        }

    }
}

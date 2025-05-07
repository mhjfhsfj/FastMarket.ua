using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastMarketBackEnd.Data;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;
using FastMarketBackEnd.services;
using FastMarketBackEnd.Utility;

namespace FastMarketBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<OrderController> _logger;
        private readonly OrderService _orderService;
        private readonly LiqPayService _liqPayService;

        

        public OrderController(ApplicationContext context, ILogger<OrderController> logger, OrderService orderService, LiqPayService liqPayService)
        {
            _context = context;
            _logger = logger;
            _orderService = orderService;
            _liqPayService = liqPayService;
        }
        
        // GET: api/Order
        [HttpGet(nameof(GetAllOrders))]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetAllOrders()
        {
            try
            {
                return await _orderService.GetOrdersAsync();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet(nameof(GetOrdersByUserId)+"/"+"{userId}")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrdersByUserId(int userId)
        {
            try
            {
                return await _orderService.GetOrdersByUserIdAsync(userId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet(nameof(GetOrdersBySellerId)+"/"+"{sellerId}")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrdersBySellerId(int sellerId)
        {
            try
            {
                return await _orderService.GetOrdersBySellerIdAsync(sellerId);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet(nameof(GetOrderById)+"/"+"{Id}")]
        public async Task<ActionResult<GetOrderDTO>> GetOrderById(int Id)
        {
            try
            {
                return await _orderService.GetOrderByIdAsync(Id);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(nameof(ChangeOrder)+"/{id}")]
        public async Task<IActionResult> ChangeOrder(int id, OrderChangeDTO order)
        {
            try
            {
                await _orderService.ChangeOrderAsync(id,order);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
        
        [HttpPut(nameof(ChangeOrderStatus) + "/{id}")]
        public async Task<IActionResult> ChangeOrderStatus(int id, StatusOrder statusOrder)
        {
            try
            {
                await _orderService.ChangeOrderStatusAsync(id,statusOrder);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(nameof(CreateOrder))]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> CreateOrder(CreateOrderRequest order)
        {
            try
            {
                return await _orderService.CreateOrderAsync(order);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Order/5
        [HttpDelete(nameof(DeleteOrder) + "/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }
        
        [HttpGet("liqpay/form")]
        public IActionResult GetLiqPayForm(int orderId, decimal amount)
        {
            var (data, signature) = _liqPayService.CreatePayment(amount, orderId.ToString(), "Оплата товару");

            var formHtml = $@"
        <form method='POST' action='https://www.liqpay.ua/api/3/checkout' accept-charset='utf-8'>
            <input type='hidden' name='data' value='{data}' />
            <input type='hidden' name='signature' value='{signature}' />
            <input type='submit' value='Оплатити LiqPay' />
        </form>";

            return Content(formHtml, "text/html");
        }
        
        [HttpPost("payment-callback")]
        public async Task<IActionResult> Callback([FromForm] string data, [FromForm] string signature)
        {
            var decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            
            var paymentInfo = JsonSerializer.Deserialize<LiqPayCallback>(decodedJson);

            if (paymentInfo.status == "success")
            {
                await _orderService.ChangeOrderPaymentStatusAsync(Convert.ToInt32(paymentInfo.order_id),
                    PaymentStatus.Success);
            }
            return Ok();
        }
    }
}

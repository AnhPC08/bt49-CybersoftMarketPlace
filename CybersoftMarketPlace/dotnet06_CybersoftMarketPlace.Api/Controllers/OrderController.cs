using System.Threading.Tasks;
using backend_netcore_dotnet06.Helper;  
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace dotnet06_CybersoftMarketPlace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        //Chỉ làm việc với service, không làm việc trực tiếp với repository
        public OrderController(IUserService userService, IOrderService orderService, ICartService cartService)
        {
            _userService = userService;
            _orderService = orderService;
            _cartService = cartService;
        }

        [HttpPost("create-order")]
        [Authorize]
        public async Task<IActionResult> CreateOrder()
        {
            //Lấy ra userid
            string userId = HttpContext.User?.Identity?.Name ?? string.Empty;

            int cartId = await _cartService.GetCartIdByUserId(userId);

            var response = await _orderService.CreateOrderAsync(userId, cartId);
            return Ok(response);


        }

        /// <summary>
        /// Lấy danh sách đơn mua của chính người dùng đang đăng nhập.
        /// </summary>
        /// <returns>Danh sách đơn hàng kèm các sản phẩm trong đơn</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HTTPResponseData<List<OrderHistoryDTO>>))]
        [Authorize]
        [HttpGet("myOrders")]
        public async Task<IActionResult> MyOrders()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            HTTPResponseData<List<OrderHistoryDTO>>? response = await _orderService.GetMyOrdersAsync(token);
            return StatusCode(response.statusCode, response);
        }

        /// <summary>
        /// Lấy chi tiết một đơn hàng của chính người dùng đang đăng nhập.
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns>Chi tiết đơn hàng</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HTTPResponseData<OrderDetailDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(HTTPResponseData<OrderDetailDTO>))]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            HTTPResponseData<OrderDetailDTO>? response = await _orderService.GetOrderDetailAsync(token, id);
            return StatusCode(response.statusCode, response);
        }


    }
}

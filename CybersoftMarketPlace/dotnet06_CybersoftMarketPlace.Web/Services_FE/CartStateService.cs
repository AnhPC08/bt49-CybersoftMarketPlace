using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System.Text.Json;

public class CartStateService
{
    private readonly ILocalStorageService _localStorageService;

    private HubConnection? _hubClient;

    public CartDTO cartStateDTO { get; set; } = new CartDTO();

    public CartStateService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }


    public async Task StartHubConnectionAsync()
    {
        //Đã kết nối rồi thì không kết nối lại
        if (_hubClient != null)
        {
            return;
        }

        //Chưa đăng nhập thì không kết nối hub (các hàm hub đều có [Authorize])
        var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");
        if (string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine(@"Chưa đăng nhập - bỏ qua kết nối cart-hub");
            return;
        }

        //đưa accessToken vào header Authorization để xác thực userId khi kết nối websocket
        //LƯU Ý: trả về token thô, SignalR tự thêm tiền tố "Bearer " khi gắn header
        _hubClient = new HubConnectionBuilder().WithUrl("http://localhost:1234/cart-hub", options =>
        {
            options.AccessTokenProvider = async () =>
            {
                return await _localStorageService.GetItemAsync<string>("accessToken");
            };
        })
        .WithAutomaticReconnect()
        .Build();

        //Đăng ký handler TRƯỚC khi StartAsync để không mất message server gửi ngay sau khi connect
        _hubClient.On<CartDTO>("GetCartByUserId", (CartDTO cartDTO) =>
        {
            //Cập nhật dữ liệu giỏ hàng vào state management
            cartStateDTO = cartDTO ?? new CartDTO();

            Console.WriteLine(@$"Giỏ hàng : {JsonSerializer.Serialize(cartDTO)}");

            //Load luồng dữ liệu vào component
            StateHasChanged();
        });

        await _hubClient.StartAsync();
    }


    public async Task ChangeQuantityVariantInCart(int productVariantId, int newQuantity)
    {
        if (_hubClient == null)
        {
            return;
        }

        await _hubClient.InvokeAsync("ChangeQuantityVariantInCart", productVariantId, newQuantity);
    }



    public async Task AddItemToCart(int productVariantId, int quantity)
    {
        if (_hubClient == null)
        {
            return;
        }

        await _hubClient.InvokeAsync("HandleAddItemToCart", productVariantId, quantity);
    }


    //gọi load giỏ hàng (gọi api CartHub)
    public async Task LoadCartAsync()
    {
        if (_hubClient == null)
        {
            return;
        }

        await _hubClient.InvokeAsync(methodName: "HandleGetCartByUserId");
    }


    public async Task StopHubConnectionAsync()
    {
        if (_hubClient == null)
        {
            return;
        }

        //Huỷ kết nối websocket khi component bị hủy
        await _hubClient.StopAsync();
        await _hubClient.DisposeAsync();
        _hubClient = null;
    }
    public event Action? OnChange;
    private void StateHasChanged() => OnChange?.Invoke();
    // Place your HTTP methods below
}

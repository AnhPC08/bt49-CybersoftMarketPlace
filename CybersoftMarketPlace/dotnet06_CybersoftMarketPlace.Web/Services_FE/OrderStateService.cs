using System.Net.Http.Json;
using Microsoft.JSInterop; // ILocalStorageService nằm trong namespace này

// State management cho màn "Đơn mua" trong trang Hồ sơ.
// Giữ đúng vai trò như ProductStateService/CartStateService: trang .razor chỉ gọi service,
// logic gọi API + giữ dữ liệu nằm ở đây.
public class OrderStateService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;

    public List<OrderHistoryDTO> MyOrders { get; private set; } = new();
    public bool IsLoading { get; private set; }

    public OrderStateService(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClientFactory.CreateClient("CybersoftMarketplaceApi");
    }

    public event Action? OnChange;
    private void StateHasChanged() => OnChange?.Invoke();

    // Lấy danh sách đơn mua của người đang đăng nhập.
    public async Task LoadMyOrdersAsync()
    {
        IsLoading = true;
        StateHasChanged();
        try
        {
            string? token = await _localStorageService.GetItemAsync<string>("accessToken");
            if (string.IsNullOrEmpty(token))
            {
                MyOrders = new();
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HTTPResponseData<List<OrderHistoryDTO>>? data =
                await _httpClient.GetFromJsonAsync<HTTPResponseData<List<OrderHistoryDTO>>>("/api/Order/myOrders");

            MyOrders = data?.DataResponse ?? new();
        }
        catch
        {
            // Lỗi mạng / token hỏng -> để danh sách rỗng, trang tự hiện "chưa có đơn".
            MyOrders = new();
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }
}

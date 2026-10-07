using Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;
public class UserStateService
{
    private readonly ILocalStorageService _localStorageService;
    public string accessToken = "";
    private readonly HttpClient _httpClient;

    public ProfileUserDTO? CurrentUser { get; private set; }
    public readonly NavigationManager _navigationManager;

    public UserStateService(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory,NavigationManager nav)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClientFactory.CreateClient("CybersoftMarketplaceApi");
        _navigationManager = nav; //Service dùng để chuyển hướng trang
    }

    public Action OnChange { get; set; }
    public void StateHasChanged() => OnChange?.Invoke();

    public async Task LoginAsync(UserLoginDTO userLogin)
    {
        //Gọi api từ backend để đăng nhập
        var response = await _httpClient.PostAsJsonAsync("/api/User/Login", userLogin);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<HTTPResponseData<string>>();
            if (responseData != null && responseData.statusCode == 200)
            {
                accessToken = responseData.DataResponse;
                await _localStorageService.SetItemAsync("accessToken", accessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Gọi hàm getprofile khi đăng nhập
                await GetProfileAsync();
                _navigationManager.NavigateTo("/profile"); //Chuyển hướng về trang chủ sau khi đăng nhập thành công

                StateHasChanged();
                
            }
        }
    }

    public async Task GetProfileAsync()
    {
        //Add header token
        string? token = await _localStorageService.GetItemAsync<string>("accessToken");
        Console.WriteLine($@"{token} - tokendemo");
        if (token != null)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //Gọi api từ backend để lấy thông tin người dùng
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/User/getProfile", null);

            if (response.IsSuccessStatusCode)
            {
                HTTPResponseData<ProfileUserDTO>? responseData = await response.Content.ReadFromJsonAsync<HTTPResponseData<ProfileUserDTO>>();
                if (responseData != null && responseData.statusCode == 200)
                {
                    CurrentUser = responseData.DataResponse;
                    accessToken = token;
                    StateHasChanged();
                }
            }
        }else
        {
            CurrentUser = null;
            accessToken = "";
            StateHasChanged();
        }
    }

    public async Task LogoutAsync()
    {
        accessToken = "";
        CurrentUser = null;
        await _localStorageService.RemoveItemAsync("accessToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
        StateHasChanged();
    }

    // ============ BUỔI 49: các hành động ở trang Hồ sơ ============
    // Thông báo của lần gọi API gần nhất (để trang hiển thị alert xanh/đỏ).
    public string ActionMessage { get; private set; } = "";

    // Gắn token vào header Authorization trước mỗi lần gọi API cần đăng nhập.
    private async Task SetAuthHeaderAsync()
    {
        string? token = await _localStorageService.GetItemAsync<string>("accessToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    // Cập nhật hồ sơ. Thành công thì đồng bộ lại CurrentUser từ dữ liệu server trả về.
    public async Task<bool> UpdateProfileAsync(UpdateProfileDTO model)
    {
        await SetAuthHeaderAsync();
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/User/updateProfile", model);
        HTTPResponseData<ProfileUserDTO>? data =
            await response.Content.ReadFromJsonAsync<HTTPResponseData<ProfileUserDTO>>();

        ActionMessage = data?.Message ?? "Có lỗi xảy ra, vui lòng thử lại.";
        bool ok = response.IsSuccessStatusCode && data != null && data.statusCode == 200 && data.DataResponse != null;
        if (ok)
        {
            CurrentUser = data!.DataResponse;
        }
        StateHasChanged();
        return ok;
    }

    // Đổi mật khẩu. Không đụng tới CurrentUser.
    public async Task<bool> ChangePasswordAsync(ChangePasswordDTO model)
    {
        await SetAuthHeaderAsync();
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/User/changePassword", model);
        HTTPResponseData<string>? data =
            await response.Content.ReadFromJsonAsync<HTTPResponseData<string>>();

        ActionMessage = data?.Message ?? "Có lỗi xảy ra, vui lòng thử lại.";
        bool ok = response.IsSuccessStatusCode && data != null && data.statusCode == 200;
        StateHasChanged();
        return ok;
    }

    // Đổi ảnh đại diện (dán link). Thành công thì cập nhật luôn ảnh trên CurrentUser.
    public async Task<bool> UpdateAvatarAsync(string avatarUrl)
    {
        await SetAuthHeaderAsync();
        UpdateAvatarDTO model = new UpdateAvatarDTO { AvatarUrl = avatarUrl };
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/User/updateAvatar", model);
        HTTPResponseData<string>? data =
            await response.Content.ReadFromJsonAsync<HTTPResponseData<string>>();

        ActionMessage = data?.Message ?? "Có lỗi xảy ra, vui lòng thử lại.";
        bool ok = response.IsSuccessStatusCode && data != null && data.statusCode == 200;
        if (ok && CurrentUser != null)
        {
            CurrentUser.AvatarUrl = avatarUrl;
        }
        StateHasChanged();
        return ok;
    }

}
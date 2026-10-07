# CybersoftMarketPlace - Cách chạy dự án

Dự án marketplace (.NET 10, kiến trúc 4 tầng): Api + Application + Infrastructure + Web (Blazor Server).
Bài tập buổi 49: làm trang Profile (xem phần cuối).

## Yêu cầu
- .NET 10 SDK
- Không cần cài SQL Server: chuỗi kết nối trỏ tới DB remote của thầy, đã cấu hình sẵn trong
  dotnet06_CybersoftMarketPlace.Api/appsettings.json (phải có mạng).

## Chạy
Mở 2 cửa sổ terminal tại thư mục gốc dự án.

Terminal 1 - API (chạy trước):
```
dotnet run --project dotnet06_CybersoftMarketPlace.Api --launch-profile http
```
API: http://localhost:1234 (Swagger ở trang chủ /). Dừng: Ctrl + C.

Terminal 2 - Web:
```
dotnet run --project dotnet06_CybersoftMarketPlace.Web
```
Web: http://localhost:5290. Dừng: Ctrl + C.

Lưu ý: Web gọi API ở cổng 1234 nên phải bật API trước.

## Build kiểm tra (không chạy)
```
dotnet build dotnet06_CybersoftMarketPlace.Api
dotnet build dotnet06_CybersoftMarketPlace.Web
```

## Test API trên Swagger
1. Gọi POST /api/User/Login để lấy token.
2. Bấm nút Authorize (góc trên Swagger), dán token vào.
3. Test các endpoint cần đăng nhập.

## Các endpoint của bài 49
- PUT  /api/User/updateProfile   - cập nhật họ tên, email, SĐT, địa chỉ
- PUT  /api/User/changePassword  - đổi mật khẩu (cần mật khẩu cũ)
- PUT  /api/User/updateAvatar    - đổi ảnh đại diện (dán link)
- GET  /api/Order/myOrders       - danh sách đơn mua của mình
- GET  /api/Order/{id}           - chi tiết 1 đơn hàng

Trang giao diện tương ứng: http://localhost:5290/profile

## User Test:
Hoang Anh | hoanganhtest@gmail.com| Test@1234 | 0900000002

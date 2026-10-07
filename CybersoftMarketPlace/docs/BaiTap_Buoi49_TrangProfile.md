# Bài tập buổi 49 — Làm cho trang Profile sống dậy

> **Dự án:** CybersoftMarketPlace · .NET 10 · kiến trúc 4 tầng
> **Làm cả:** Backend (API) và Frontend (Blazor)
> **Thời gian:** 1 tuần

Buổi 47 lớp mình đã làm xong đăng ký — đăng nhập — sinh JWT, buổi 48 làm trang chi tiết sản phẩm. Trang `/profile` thì mình đã dựng sẵn giao diện bằng Bootstrap cho các bạn rồi, nhưng nói thật thì nó mới chỉ là cái vỏ: bấm vào đâu cũng không có gì xảy ra, gõ vào ô input xong bấm *Lưu thay đổi* thì… cũng không lưu gì cả.

Việc của các bạn tuần này là biến từng nút bấm trong cái vỏ đó thành chức năng chạy thật — từ giao diện, xuống API, xuống tận database rồi quay ngược lên.

Mở file [Profile.razor](../dotnet06_CybersoftMarketPlace.Web/Pages/Profile.razor) ra, ngó qua một lượt, rồi nhìn cái hình bên dưới.

---

## 1. Giao diện hiện có — và những chỗ cần làm

<svg viewBox="0 0 980 520" style="width:100%;height:auto;font-family:-apple-system,Helvetica,Arial,sans-serif" xmlns="http://www.w3.org/2000/svg">
<defs><marker id="ar" viewBox="0 0 10 10" refX="9" refY="5" markerWidth="7" markerHeight="7" orient="auto-start-reverse"><path d="M0,0 L10,5 L0,10 z" fill="#d0021b"/></marker></defs>
<rect x="190" y="30" width="600" height="426" rx="6" fill="#ffffff" stroke="#b6bec7"/>
<rect x="190" y="30" width="600" height="26" fill="#eef1f4" stroke="#b6bec7"/>
<circle cx="204" cy="43" r="3.5" fill="#cfd6dd"/><circle cx="215" cy="43" r="3.5" fill="#cfd6dd"/><circle cx="226" cy="43" r="3.5" fill="#cfd6dd"/>
<text x="244" y="47" font-size="11" fill="#6b7480">localhost:5001/profile</text>
<rect x="205" y="76" width="170" height="150" rx="5" fill="#ffffff" stroke="#dde2e7"/>
<circle cx="290" cy="112" r="22" fill="#eef1f4" stroke="#cfd6dd"/>
<text x="290" y="148" font-size="13" font-weight="600" text-anchor="middle" fill="#1b1f23">Nguyễn Văn A</text>
<text x="290" y="164" font-size="10.5" fill="#6b7480" text-anchor="middle">nguyenvana@gmail.com</text>
<rect x="230" y="176" width="120" height="24" rx="12" fill="#ffffff" stroke="#d0021b"/>
<text x="290" y="192" font-size="10.5" fill="#d0021b" text-anchor="middle">Đổi ảnh đại diện</text>
<rect x="205" y="238" width="170" height="204" rx="5" fill="#ffffff" stroke="#dde2e7"/>
<rect x="205" y="238" width="170" height="34" rx="5" fill="#d0021b"/>
<text x="220" y="260" font-size="13" fill="#ffffff">Hồ sơ của tôi</text>
<text x="220" y="294" font-size="13" fill="#1b1f23">Đổi mật khẩu</text>
<text x="220" y="328" font-size="13" fill="#1b1f23">Địa chỉ giao hàng</text>
<text x="220" y="362" font-size="13" fill="#1b1f23">Đơn mua</text>
<text x="220" y="396" font-size="13" fill="#1b1f23">Kho voucher</text>
<text x="220" y="430" font-size="13" fill="#d0021b">Đăng xuất</text>
<line x1="205" y1="272" x2="375" y2="272" stroke="#eceff2"/><line x1="205" y1="306" x2="375" y2="306" stroke="#eceff2"/><line x1="205" y1="340" x2="375" y2="340" stroke="#eceff2"/><line x1="205" y1="374" x2="375" y2="374" stroke="#eceff2"/><line x1="205" y1="408" x2="375" y2="408" stroke="#eceff2"/>
<rect x="390" y="76" width="385" height="360" rx="5" fill="#ffffff" stroke="#dde2e7"/>
<text x="406" y="100" font-size="13" font-weight="600" fill="#1b1f23">Hồ sơ của tôi</text>
<text x="406" y="115" font-size="10" fill="#6b7480">Quản lý thông tin hồ sơ để bảo mật tài khoản</text>
<line x1="390" y1="126" x2="775" y2="126" stroke="#eceff2"/>
<text x="470" y="153" font-size="11" fill="#6b7480" text-anchor="end">Mã người dùng</text>
<rect x="482" y="138" width="120" height="22" rx="4" fill="#f0f2f4" stroke="#dde2e7"/>
<text x="492" y="153" font-size="9.5" fill="#6b7480" font-family="monospace">3f7a…9c21</text>
<text x="470" y="189" font-size="11" fill="#6b7480" text-anchor="end">Họ và tên</text>
<rect x="482" y="172" width="230" height="26" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="494" y="189" font-size="11" fill="#33383d">Nguyễn Văn A</text>
<text x="470" y="225" font-size="11" fill="#6b7480" text-anchor="end">Email</text>
<rect x="482" y="208" width="230" height="26" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="494" y="225" font-size="11" fill="#33383d">nguyenvana@gmail.com</text>
<text x="470" y="261" font-size="11" fill="#6b7480" text-anchor="end">Số điện thoại</text>
<rect x="482" y="244" width="230" height="26" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="494" y="261" font-size="11" fill="#33383d">0901 234 567</text>
<text x="470" y="296" font-size="11" fill="#6b7480" text-anchor="end">Địa chỉ</text>
<rect x="482" y="280" width="230" height="38" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="494" y="297" font-size="11" fill="#33383d">12 Nguyễn Văn Bảo, Gò Vấp</text>
<text x="470" y="350" font-size="11" fill="#6b7480" text-anchor="end">Ảnh đại diện</text>
<circle cx="497" cy="345" r="15" fill="#eef1f4" stroke="#cfd6dd"/>
<rect x="520" y="333" width="192" height="24" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="530" y="349" font-size="9.5" fill="#98a1ab">https://…/avatar.png</text>
<line x1="406" y1="378" x2="760" y2="378" stroke="#eceff2"/>
<rect x="482" y="392" width="110" height="28" rx="4" fill="#d0021b"/>
<text x="537" y="411" font-size="12.5" fill="#ffffff" text-anchor="middle">Lưu thay đổi</text>
<rect x="602" y="392" width="70" height="28" rx="4" fill="#ffffff" stroke="#b6bec7"/>
<text x="637" y="411" font-size="12.5" fill="#6b7480" text-anchor="middle">Huỷ</text>
<path d="M161,188 H226" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar)"/>
<circle cx="150" cy="188" r="10" fill="#d0021b"/><text x="150" y="192" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">1</text>
<text x="134" y="184" font-size="13" fill="#1b1f23" text-anchor="end" font-weight="600">Nút đổi ảnh</text>
<text x="134" y="199" font-size="12.5" fill="#57606a" text-anchor="end">dùng chung API với 6</text>
<path d="M161,306 H201" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar)"/>
<circle cx="150" cy="306" r="10" fill="#d0021b"/><text x="150" y="310" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">2</text>
<text x="134" y="295" font-size="13" fill="#1b1f23" text-anchor="end" font-weight="600">6 mục = 6 màn hình</text>
<text x="134" y="310" font-size="12.5" fill="#57606a" text-anchor="end">đang là href="#"</text>
<text x="134" y="325" font-size="12.5" fill="#57606a" text-anchor="end">bấm không đi đâu cả</text>
<path d="M161,425 H201" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar)"/>
<circle cx="150" cy="425" r="10" fill="#d0021b"/><text x="150" y="429" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">3</text>
<text x="134" y="421" font-size="13" fill="#1b1f23" text-anchor="end" font-weight="600">Đăng xuất</text>
<text x="134" y="436" font-size="12.5" fill="#57606a" text-anchor="end">xoá token → /login</text>
<path d="M789,149 H608" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar)"/>
<circle cx="800" cy="149" r="10" fill="#d0021b"/><text x="800" y="153" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">4</text>
<text x="816" y="145" font-size="13" fill="#1b1f23" font-weight="600">Chỉ để xem</text>
<text x="816" y="160" font-size="12.5" fill="#57606a">Id lấy từ token</text>
<path d="M726,172 h-8 M726,172 V318 M726,318 h-8" stroke="#d0021b" stroke-width="1.4" fill="none"/>
<path d="M789,245 H732" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar)"/>
<circle cx="800" cy="245" r="10" fill="#d0021b"/><text x="800" y="249" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">5</text>
<text x="816" y="228" font-size="13" fill="#1b1f23" font-weight="600">Các ô được sửa</text>
<text x="816" y="243" font-size="12.5" fill="#57606a">→ UpdateProfileDTO</text>
<text x="816" y="258" font-size="12.5" fill="#57606a">nhớ @bind 2 chiều</text>
<path d="M789,346 H718" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar)"/>
<circle cx="800" cy="346" r="10" fill="#d0021b"/><text x="800" y="350" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">6</text>
<text x="816" y="342" font-size="13" fill="#1b1f23" font-weight="600">Link ảnh</text>
<text x="816" y="357" font-size="12.5" fill="#57606a">→ UpdateAvatarDTO</text>
<path d="M505,476 L534,426" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar)"/>
<circle cx="500" cy="487" r="10" fill="#d0021b"/><text x="500" y="491" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">7</text>
<text x="518" y="483" font-size="13" fill="#1b1f23" font-weight="600">Lưu thay đổi → PUT /api/User/updateProfile</text>
<text x="518" y="498" font-size="12.5" fill="#57606a">Huỷ → nạp lại dữ liệu gốc, không gọi API</text>
</svg>

Đi từng mũi tên một nhé.

### 1 — Nút *Đổi ảnh đại diện*

Hiện tại bấm vào không có gì. Bài này chưa cần upload file thật, cứ cho người dùng dán một cái link ảnh vào ô ở mũi tên **6** là được, nút này chỉ cần focus vào ô đó hoặc mở một modal nhỏ. Ai muốn làm upload `IFormFile` thật thì để dành làm phần nâng cao.

### 2 — Menu 6 mục bên trái

Đây là chỗ đáng làm nhất. Mở [Profile.razor:26-43](../dotnet06_CybersoftMarketPlace.Web/Pages/Profile.razor#L26-L43) sẽ thấy cả 6 mục đều là `<a href="#">` — tức là trang trí thôi. Mỗi mục là một màn hình:

| Mục | Phải làm gì |
|---|---|
| Hồ sơ của tôi | màn hình đang có sẵn |
| Đổi mật khẩu | tự dựng thêm, xem hình ở mục 2 |
| Địa chỉ giao hàng | phần nâng cao |
| Đơn mua | tự dựng thêm, xem hình ở mục 2 |
| Kho voucher | phần nâng cao |
| Đăng xuất | mũi tên 3 |

Chọn 1 trong 2 cách làm: hoặc dùng một biến `string tabDangChon` với `@onclick` rồi `@if` để ẩn hiện từng khối (dễ, làm nhanh); hoặc tách hẳn route riêng `/profile`, `/profile/password`, `/profile/orders`… mỗi màn một file `.razor` dùng chung layout (đúng chuẩn hơn, mình khuyến khích cách này).

### 3 — Đăng xuất

Cái này nhẹ nhất, không cần API. `UserStateService` đã có sẵn `LogoutAsync()` rồi — gọi nó để xoá token trong localStorage, rồi `NavigateTo("/login")`. Nhớ kiểm tra lại: sau khi đăng xuất mà gõ tay `/profile` vào thanh địa chỉ thì phải bị đá ra, không được để trang trắng hay văng lỗi null.

### 4 — Ô *Mã người dùng*

Ô này **chỉ để xem**, không cho sửa, và cũng đừng gửi nó lên server. `Id` của người đang đăng nhập nằm sẵn trong token rồi, server tự đọc ra bằng `_jwtAuthService.DecodePayloadTokenId(token)` — y hệt cách [UserService.cs:37](../dotnet06_CybersoftMarketPlace.Application/Services/UserService.cs#L37) đang làm với `GetProfileAsync`.

Nghĩ thử xem: nếu API nhận `Id` do client gửi lên thì chuyện gì xảy ra khi ai đó sửa `Id` thành của người khác rồi bấm Lưu?

### 5 — Bốn ô *Họ và tên / Email / SĐT / Địa chỉ*

Đây là phần chính. Hai việc phải làm:

Ở FE, các ô này đang dùng `value="@_userStateService.CurrentUser.FullName"` — một chiều, gõ vào không lưu lại được gì. Phải đổi sang `@bind`. Và nghĩ tiếp: bind thẳng vào `CurrentUser` hay copy ra một object `UpdateProfileDTO` riêng rồi bind vào đó? Cứ thử cả hai rồi bấm nút *Huỷ* xem cách nào khôi phục dữ liệu dễ hơn.

Ở BE, viết `PUT /api/User/updateProfile`. DTO gợi ý:

```csharp
public class UpdateProfileDTO
{
    public string FullName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    // Còn Email thì sao? Cho sửa hay không cho sửa?
    // Nếu cho sửa thì phải check trùng như lúc Register — nhớ luôn cả check trùng Phone.
    // Còn Id thì tuyệt đối không đưa vào đây, lý do đã nói ở mũi tên 4.
}
```

### 6 — Ô link ảnh đại diện

Tách riêng một API `PUT /api/User/updateAvatar` với `UpdateAvatarDTO` chỉ đúng một field `AvatarUrl`. Có bạn sẽ hỏi: sao không gộp luôn vào `UpdateProfileDTO` cho gọn? Gộp cũng được, nhưng khi lên phần nâng cao làm upload file thật thì avatar đi đường riêng (multipart) chứ không đi chung JSON với các field kia nữa — tách sẵn từ giờ đỡ phải sửa lại.

### 7 — Hai nút cuối trang

*Lưu thay đổi* gọi API, còn *Huỷ* thì không gọi API nào cả, chỉ nạp lại dữ liệu cũ vào form. Trong lúc đang gọi API nhớ cho nút `disabled` và hiện spinner — cách làm loading đã có sẵn ở [ProductDetail.razor](../dotnet06_CybersoftMarketPlace.Web/Pages/ProductDetail.razor) buổi trước. Gọi xong: thành công thì alert xanh, thất bại thì alert đỏ và hiện đúng `Message` mà API trả về, đừng nuốt lỗi.

---

## 2. Hai màn hình phải tự dựng thêm

Hai màn này chưa có gì cả, các bạn tự vẽ bằng Bootstrap. Đây là hình mình phác cho dễ hình dung, không cần giống y hệt:

<svg viewBox="0 0 980 300" style="width:100%;height:auto;font-family:-apple-system,Helvetica,Arial,sans-serif" xmlns="http://www.w3.org/2000/svg">
<defs><marker id="ar2" viewBox="0 0 10 10" refX="9" refY="5" markerWidth="7" markerHeight="7" orient="auto-start-reverse"><path d="M0,0 L10,5 L0,10 z" fill="#d0021b"/></marker></defs>
<rect x="20" y="30" width="450" height="212" rx="6" fill="#ffffff" stroke="#b6bec7"/>
<text x="40" y="58" font-size="13" font-weight="600" fill="#1b1f23">Đổi mật khẩu</text>
<line x1="20" y1="70" x2="470" y2="70" stroke="#eceff2"/>
<text x="192" y="103" font-size="11" fill="#6b7480" text-anchor="end">Mật khẩu hiện tại</text>
<rect x="204" y="86" width="234" height="26" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="216" y="104" font-size="13" fill="#98a1ab">••••••••</text>
<text x="192" y="139" font-size="11" fill="#6b7480" text-anchor="end">Mật khẩu mới</text>
<rect x="204" y="122" width="234" height="26" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="216" y="140" font-size="13" fill="#98a1ab">••••••••</text>
<text x="192" y="175" font-size="11" fill="#6b7480" text-anchor="end">Nhập lại mật khẩu mới</text>
<rect x="204" y="158" width="234" height="26" rx="4" fill="#fbfcfd" stroke="#dde2e7"/>
<text x="216" y="176" font-size="13" fill="#98a1ab">••••••••</text>
<rect x="204" y="198" width="110" height="28" rx="4" fill="#d0021b"/>
<text x="259" y="217" font-size="12.5" fill="#ffffff" text-anchor="middle">Xác nhận</text>
<path d="M40,268 V248" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar2)"/>
<circle cx="40" cy="280" r="10" fill="#d0021b"/><text x="40" y="284" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">8</text>
<text x="58" y="278" font-size="13" fill="#1b1f23" font-weight="600">PUT /api/User/changePassword</text>
<text x="58" y="292" font-size="12.5" fill="#57606a">ChangePasswordDTO — bắt buộc có mật khẩu cũ</text>
<rect x="510" y="30" width="450" height="212" rx="6" fill="#ffffff" stroke="#b6bec7"/>
<text x="530" y="58" font-size="13" font-weight="600" fill="#1b1f23">Đơn mua</text>
<line x1="510" y1="70" x2="960" y2="70" stroke="#eceff2"/>
<rect x="530" y="82" width="410" height="70" rx="5" fill="#ffffff" stroke="#dde2e7"/>
<text x="542" y="101" font-size="11" fill="#6b7480">#1024 · 28/07/2026 · Đang giao</text>
<text x="928" y="101" font-size="12.5" font-weight="600" fill="#d0021b" text-anchor="end">1.250.000 đ</text>
<line x1="530" y1="110" x2="940" y2="110" stroke="#eceff2"/>
<rect x="542" y="118" width="26" height="26" rx="3" fill="#eef1f4" stroke="#cfd6dd"/>
<text x="578" y="129" font-size="10.5" fill="#1b1f23">Áo thun nam — size L</text>
<text x="578" y="143" font-size="10" fill="#6b7480">x2 · 250.000 đ</text>
<rect x="836" y="118" width="100" height="24" rx="4" fill="#ffffff" stroke="#b6bec7"/>
<text x="886" y="134" font-size="10.5" fill="#57606a" text-anchor="middle">Xem chi tiết</text>
<rect x="530" y="162" width="410" height="70" rx="5" fill="#ffffff" stroke="#dde2e7"/>
<text x="542" y="181" font-size="11" fill="#6b7480">#1019 · 22/07/2026 · Hoàn thành</text>
<text x="928" y="181" font-size="12.5" font-weight="600" fill="#d0021b" text-anchor="end">480.000 đ</text>
<line x1="530" y1="190" x2="940" y2="190" stroke="#eceff2"/>
<rect x="542" y="198" width="26" height="26" rx="3" fill="#eef1f4" stroke="#cfd6dd"/>
<text x="578" y="209" font-size="10.5" fill="#1b1f23">Tai nghe Bluetooth</text>
<text x="578" y="223" font-size="10" fill="#6b7480">x1 · 480.000 đ</text>
<rect x="836" y="198" width="100" height="24" rx="4" fill="#ffffff" stroke="#b6bec7"/>
<text x="886" y="214" font-size="10.5" fill="#57606a" text-anchor="middle">Xem chi tiết</text>
<path d="M530,268 V248" stroke="#d0021b" stroke-width="1.6" fill="none" marker-end="url(#ar2)"/>
<circle cx="530" cy="280" r="10" fill="#d0021b"/><text x="530" y="284" font-size="13" font-weight="700" fill="#ffffff" text-anchor="middle">9</text>
<text x="548" y="278" font-size="13" fill="#1b1f23" font-weight="600">GET /api/Order/myOrders</text>
<text x="548" y="292" font-size="12.5" fill="#57606a">OrderHistoryDTO chứa List&lt;OrderHistoryItemDTO&gt;</text>
</svg>

### 8 — Đổi mật khẩu

Ba ô, và cái ô *Mật khẩu hiện tại* là bắt buộc phải có — đừng bỏ đi cho tiện. Cứ thử nghĩ: nếu bỏ đi thì một người mượn được máy bạn lúc bạn quên đăng xuất sẽ làm được gì?

```csharp
public class ChangePasswordDTO
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
```

Bên service dùng `HelperFunction.VerifyPassword(old, user.PasswordHash)` để đối chiếu mật khẩu cũ, rồi `HelperFunction.HashPassword(new)` để lưu cái mới — hai hàm này đã viết sẵn trong [HelperFunc.cs](../dotnet06_CybersoftMarketPlace.Application/Helper/HelperFunc.cs), cứ dùng lại. Còn `ConfirmPassword` thì kiểm ở FE hay BE? Câu trả lời là cả hai, và các bạn phải giải thích được vì sao.

### 9 — Đơn mua

Lấy danh sách đơn hàng của chính người đang đăng nhập (lại là `Id` lấy từ token, không nhận từ client). Bảng `Order` — `OrderItem` — `ProductVariant` — `Product` đã có sẵn dữ liệu, `OrderRepository` và `OrderItemRepository` cũng đã nằm sẵn trong UnitOfWork rồi, không phải tạo mới.

DTO thì làm theo kiểu lồng nhau, y như `ProductDetailDTO` các bạn đã viết ở buổi 48:

```csharp
public class OrderHistoryDTO
{
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Đang xử lý";  // bảng Order chưa có cột này, tự xử lý
    public List<OrderHistoryItemDTO> Items { get; set; } = new();
}

public class OrderHistoryItemDTO
{
    public int VariantId { get; set; }
    public string ProductName { get; set; } = null!;   // đi từ Variant.Product.Name
    public string VariantName { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    // SubTotal = Quantity * UnitPrice thì để BE tính sẵn hay để FE tự nhân? Chọn một và giải thích.
}
```

Một cái bẫy ở đây: nếu các bạn lặp qua `order.OrderItems` rồi lại chấm tiếp `item.Variant.Product.Name`, EF Core sẽ bắn ra một đống câu SQL con — đúng bài N+1 mà lớp đã nói. Dùng `Include()` / `ThenInclude()`, và vì đây là màn hình chỉ đọc nên nhớ thêm `AsNoTracking()`.

---

## 3. Chuyện thiết kế DTO

Phần này mình muốn các bạn nghĩ kỹ nhất, vì code thì ai cũng viết được, còn thiết kế sai DTO thì sửa rất mệt về sau.

**Nhìn giao diện để suy ra DTO, đừng nhìn bảng database.** Bảng `User` có 12 cột, nhưng màn hình *Hồ sơ của tôi* chỉ cho sửa 3–4 cái. DTO nhận vào chỉ nên có đúng 3–4 field đó thôi. Một field có mặt trong DTO đầu vào đồng nghĩa với "field này cho phép sửa" — nên đưa dư một field là mở toang một cánh cửa.

**DTO đi vào và DTO đi ra là hai class khác nhau.** `ProfileUserDTO` (server trả về) và `UpdateProfileDTO` (client gửi lên) trông na ná nhau, nhưng đừng gộp làm một. Chúng sẽ tiến hoá theo hai hướng khác nhau ngay khi thêm chức năng mới.

**Đừng bao giờ trả entity ra ngoài API.** Trả thẳng `User` là kèm luôn `PasswordHash` cho cả thế giới xem. Trong dự án này quy định rõ: entity không rời khỏi tầng Application, controller chỉ nhận DTO và trả DTO, tất cả bọc trong `HTTPResponseData<T>`.

Điền thử cái bảng này trước khi bắt tay vào code:

| Field trong `User` | Có trong `ProfileUserDTO` (trả ra)? | Có trong `UpdateProfileDTO` (nhận vào)? | Vì sao |
|---|---|---|---|
| `Id` | có | ? | |
| `Username` | ? | ? | |
| `Email` | có | ? | |
| `PasswordHash` | **không** | **không** | lộ hash là lộ mật khẩu |
| `Phone` | có | ? | |
| `Address` | có | ? | |
| `Avatar` | có | ? | |
| `CreatedAt` | ? | ? | |
| `Deleted` | ? | ? | |

---

## 4. Tổng hợp những API phải viết

| # | Endpoint | DTO vào | DTO ra | Mức |
|---|---|---|---|---|
| 1 | `PUT /api/User/updateProfile` | `UpdateProfileDTO` | `ProfileUserDTO` | bắt buộc |
| 2 | `PUT /api/User/changePassword` | `ChangePasswordDTO` | `string` | bắt buộc |
| 3 | `PUT /api/User/updateAvatar` | `UpdateAvatarDTO` | `string` | bắt buộc |
| 4 | `GET /api/Order/myOrders` | — | `List<OrderHistoryDTO>` | khá |
| 5 | `GET /api/Order/{id}` | — | `OrderDetailDTO` | khá |
| 6 | API địa chỉ giao hàng (nhiều địa chỉ, 1 mặc định) | tự thiết kế | tự thiết kế | nâng cao |
| 7 | API kho voucher | tự thiết kế | tự thiết kế | nâng cao |

Hai dòng nâng cao phải thêm bảng mới vào database — làm theo skill `/migration`, và **hỏi mình trước khi chạy `dotnet ef database update`**, đừng tự ý update.

---

## 5. Vài điều bắt buộc khi code

**Backend.** Đi đúng luồng `Controller → Service → UnitOfWork → Repository`, không được tắt đường. Controller chỉ gọi service rồi `return StatusCode(response.statusCode, response)` — thấy `if` nghiệp vụ nào trong controller là mình trừ điểm. Endpoint nào cũng phải có `[Authorize]` và `/// <summary>` cho Swagger. Trả đúng mã: 200 ok, 400 dữ liệu sai, 401 token hỏng, 404 không tìm thấy. Các câu thông báo thì thêm hằng số vào `UserResponseMessageDTO` trong [HTTPResponseData.cs](../dotnet06_CybersoftMarketPlace.Application/DTO/HTTPResponseData.cs), đừng rải chuỗi khắp nơi.

Làm xong BE thì test hết trên Swagger trước đã (nhớ bấm *Authorize* dán token vào), chạy ngon rồi mới đụng tới FE. Đừng làm song song cả hai rồi lỗi không biết lỗi ở đâu.

**Frontend.** Logic để trong `UserStateService`, file `.razor` chỉ gọi service — đúng như cách lớp đang làm với `LoginAsync` và `GetProfileAsync`. Thêm vào đó mấy hàm kiểu:

```csharp
public async Task<bool> UpdateProfileAsync(UpdateProfileDTO model)   { ... }
public async Task<bool> ChangePasswordAsync(ChangePasswordDTO model) { ... }
public async Task<bool> UpdateAvatarAsync(string avatarUrl)          { ... }
```

Gọi API xong nhớ `StateHasChanged()` để giao diện vẽ lại — cơ chế `OnChange` đã dựng sẵn trong [UserStateService.cs](../dotnet06_CybersoftMarketPlace.Web/Services_FE/UserStateService.cs) rồi. Giao diện thì chỉ dùng HTML với Bootstrap 5 có sẵn, không thêm file CSS, không cài thư viện ngoài.

---

## 6. Chấm điểm

| Nội dung | Điểm |
|---|---|
| DTO thiết kế đúng: tách vào/ra, không lộ `PasswordHash`, không nhận `Id` từ client | 2.5 |
| Đúng kiến trúc 4 tầng, controller sạch, không đụng DbContext ngoài repository | 2.0 |
| Mũi tên 1–7 chạy thật: bấm ở FE → dữ liệu đổi trong DB → F5 vẫn còn | 2.5 |
| FE: logic nằm ở StateService, `@bind` hai chiều, có loading và báo lỗi tử tế | 1.5 |
| Bảo mật: `[Authorize]`, BCrypt, kiểm mật khẩu cũ, validate đầu vào | 1.0 |
| Đơn mua (mũi tên 9) và xử lý được N+1 | 0.5 |
| Phần nâng cao, code sạch, comment rõ ràng | +1.0 |

Trừ điểm: commit `bin/` `obj/` (−0.5) · trả entity ra API (−1.0) · lưu mật khẩu dạng plaintext (−2.0) · tự chạy `dotnet ef database update` khi chưa hỏi (−1.0).

---

## 7. Nộp bài

Tạo nhánh `buoi49_profile_<tên_của_bạn>`, commit message tiếng Việt mô tả rõ việc đã làm, kèm ảnh chụp màn hình Swagger đã test xong từng endpoint.

---

## 8. Mấy câu mình sẽ hỏi lúc chấm

1. Sao không dùng luôn entity `User` làm DTO cho nhanh?
2. `UpdateProfileDTO` của bạn có `Id` không? Nếu có, mình đổi `Id` đó thành của người khác rồi gửi lên thì chuyện gì xảy ra?
3. Đổi mật khẩu xong, token cũ còn xài được không? Theo bạn thì nên hay không nên, và làm thế nào?
4. `ProfileUserDTO` đang để `Id` kiểu `string` trong khi entity là `Guid` — vì sao vậy, có vấn đề gì không?
5. Màn *Đơn mua* của bạn chạy bao nhiêu câu SQL cho 10 đơn hàng? Mở log Serilog ra chứng minh.

/*
    Sinh link ảnh dummyimage.com cho dbo.ProductVariants

    - Text ảnh  : alias sản phẩm + tên biến thể (vd: san-pham-noi-bat-san-pham-1-option-2)
    - Màu nền   : chọn "ngẫu nhiên" từ palette 16 màu, phân bổ theo HASHBYTES(Id)
                  -> deterministic: chạy lại nhiều lần vẫn ra cùng kết quả
    - Màu chữ   : trắng / đen, chọn sẵn theo độ sáng từng màu nền
    - Kích thước: 600x400 (giống dbo.Products.Image)

    Format: https://dummyimage.com/600x400/{bg}/{fg}.png?text={slug}

    Cách dùng: chạy PHẦN 1 để xem trước, PHẦN 2 để cập nhật, PHẦN 3 để kiểm tra.
*/

USE CybersoftMarketplace;
GO

/* ---------- PHẦN 1: XEM TRƯỚC (read-only) ---------- */
;WITH Palette AS (
    SELECT * FROM (VALUES
        ( 0, 'e74c3c', 'ffffff'),   -- đỏ
        ( 1, 'c0392b', 'ffffff'),   -- đỏ đậm
        ( 2, 'e67e22', 'ffffff'),   -- cam
        ( 3, 'f39c12', '000000'),   -- cam vàng
        ( 4, 'f1c40f', '000000'),   -- vàng
        ( 5, '2ecc71', 'ffffff'),   -- xanh lá
        ( 6, '27ae60', 'ffffff'),   -- xanh lá đậm
        ( 7, '1abc9c', 'ffffff'),   -- ngọc
        ( 8, '16a085', 'ffffff'),   -- ngọc đậm
        ( 9, '3498db', 'ffffff'),   -- xanh dương
        (10, '2980b9', 'ffffff'),   -- xanh dương đậm
        (11, '9b59b6', 'ffffff'),   -- tím
        (12, '8e44ad', 'ffffff'),   -- tím đậm
        (13, '34495e', 'ffffff'),   -- xanh đen
        (14, '2c3e50', 'ffffff'),   -- xanh đen đậm
        (15, '7f8c8d', 'ffffff')    -- xám
    ) AS p(Idx, Bg, Fg)
),
Src AS (
    SELECT  v.Id,
            v.ProductId,
            v.VariantName,
            Slug     = p.Alias + '-' + REPLACE(LOWER(LTRIM(RTRIM(v.VariantName))), ' ', '-'),
            ColorIdx = CONVERT(tinyint, SUBSTRING(HASHBYTES('MD5', CAST(v.Id AS varchar(20))), 1, 1)) % 16
    FROM dbo.ProductVariants v
    JOIN dbo.Products        p ON p.Id = v.ProductId
)
SELECT TOP 20
        s.Id,
        s.ProductId,
        s.VariantName,
        NewImage = 'https://dummyimage.com/600x400/' + pl.Bg + '/' + pl.Fg + '.png?text=' + s.Slug
FROM Src s
JOIN Palette pl ON pl.Idx = s.ColorIdx
ORDER BY s.Id;
GO

/* ---------- PHẦN 2: CẬP NHẬT ---------- */
BEGIN TRANSACTION;

;WITH Palette AS (
    SELECT * FROM (VALUES
        ( 0, 'e74c3c', 'ffffff'),
        ( 1, 'c0392b', 'ffffff'),
        ( 2, 'e67e22', 'ffffff'),
        ( 3, 'f39c12', '000000'),
        ( 4, 'f1c40f', '000000'),
        ( 5, '2ecc71', 'ffffff'),
        ( 6, '27ae60', 'ffffff'),
        ( 7, '1abc9c', 'ffffff'),
        ( 8, '16a085', 'ffffff'),
        ( 9, '3498db', 'ffffff'),
        (10, '2980b9', 'ffffff'),
        (11, '9b59b6', 'ffffff'),
        (12, '8e44ad', 'ffffff'),
        (13, '34495e', 'ffffff'),
        (14, '2c3e50', 'ffffff'),
        (15, '7f8c8d', 'ffffff')
    ) AS p(Idx, Bg, Fg)
)
UPDATE v
SET    v.Image = 'https://dummyimage.com/600x400/' + pl.Bg + '/' + pl.Fg + '.png?text='
                 + p.Alias + '-' + REPLACE(LOWER(LTRIM(RTRIM(v.VariantName))), ' ', '-')
FROM   dbo.ProductVariants v
JOIN   dbo.Products        p  ON p.Id = v.ProductId
JOIN   Palette             pl ON pl.Idx = CONVERT(tinyint, SUBSTRING(HASHBYTES('MD5', CAST(v.Id AS varchar(20))), 1, 1)) % 16;

PRINT CONCAT('Số dòng đã cập nhật: ', @@ROWCOUNT);

COMMIT TRANSACTION;   -- đổi thành ROLLBACK TRANSACTION nếu chỉ muốn thử
GO

/* ---------- PHẦN 3: KIỂM TRA ---------- */
SELECT  TongDong    = COUNT(*),
        ConNull     = SUM(CASE WHEN Image IS NULL OR Image = '' THEN 1 ELSE 0 END),
        SoMauKhacNhau = COUNT(DISTINCT SUBSTRING(Image, 33, 6)),
        DoDaiToiDa  = MAX(LEN(Image))
FROM dbo.ProductVariants;

SELECT TOP 10 Id, ProductId, VariantName, Image
FROM dbo.ProductVariants
ORDER BY NEWID();
GO

/* ---------- ROLLBACK THỦ CÔNG (nếu cần trả về trạng thái cũ) ----------
    Trước khi chạy script này: 30000/30001 dòng có Image = NULL,
    riêng Id = 30001 có Image = 'https://i.pravatar.cc?u=10'

    UPDATE dbo.ProductVariants SET Image = NULL;
    UPDATE dbo.ProductVariants SET Image = 'https://i.pravatar.cc?u=10' WHERE Id = 30001;
*/

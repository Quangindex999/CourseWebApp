# NguyenNhatQuang
# 📚 HỆ THỐNG ĐĂNG KÝ KHÓA HỌC TRỰC TUYẾN

## 📝 Giới thiệu

Đây là dự án website quản lý và đăng ký khóa học trực tuyến được xây dựng bằng công nghệ ASP.NET Core MVC, Entity Framework Core, SQL Server và Bootstrap 5. Hệ thống giúp việc quản lý thông tin khóa học, tài liệu học tập, thông báo và đăng ký khóa học trở nên dễ dàng và hiệu quả, phục vụ ba nhóm người dùng chính là Admin, Giáo viên và Sinh viên.

## 🛠️ Công nghệ sử dụng

- **Backend**: ASP.NET Core MVC, Entity Framework Core, LINQ
- **Database**: SQL Server
- **Frontend**: HTML, CSS, Bootstrap 5, Razor View Engine
- **IDE**: Visual Studio 2022

## 🚀 Các chức năng chính

### 👥 Quản lý người dùng

- Đăng nhập, đăng xuất, quản lý thông tin người dùng.
- Phân quyền người dùng (Admin, Giáo viên, Sinh viên).

### 📖 Quản lý khóa học

- Thêm, sửa, xóa thông tin khóa học (Admin, Giáo viên).
- Sinh viên xem và đăng ký khóa học.

### 📂 Quản lý tài liệu khóa học

- Giáo viên upload tài liệu học tập.
- Sinh viên tải tài liệu học tập từ các khóa học đã đăng ký.

### 📢 Quản lý thông báo

- Admin và Giáo viên tạo, xem và quản lý thông báo.
- Sinh viên xem thông báo liên quan đến khóa học.

### 📝 Đăng ký khóa học

- Sinh viên đăng ký tham gia các khóa học.
- Kiểm tra trạng thái đăng ký, ngăn chặn đăng ký trùng lặp.

## 📁 Cấu trúc dự án

```
KhoaHocOnline/
├── Controllers/
│   ├── AccountController.cs
│   ├── CoursesController.cs
│   ├── CourseMaterialsController.cs
│   └── AnnouncementsController.cs
├── Models/
│   ├── User.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   ├── CourseMaterial.cs
│   └── Announcement.cs
├── Views/
│   ├── Account/
│   ├── Courses/
│   ├── CourseMaterials/
│   ├── Announcements/
│   └── Shared/
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── lib/
│   └── materials/
├── appsettings.json
├── Program.cs
└── README.md
```

## 🔧 Hướng dẫn cài đặt và chạy ứng dụng

### 📌 Bước 1: Clone repository

```bash
git clone <link-repository>
```

### 📌 Bước 2: Cài đặt database

- Tạo database trong SQL Server.
- Chỉnh sửa chuỗi kết nối trong file `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=KhoaHocOnline;Trusted_Connection=True;"
}
```

### 📌 Bước 3: Áp dụng migration

Sử dụng Package Manager Console trong Visual Studio:

```bash
Add-Migration InitialCreate
Update-Database
```

### 📌 Bước 4: Chạy ứng dụng

Mở dự án trong Visual Studio và nhấn `F5` để chạy.

- Admin đăng nhập bằng tài khoản đã tạo sẵn.
- Giáo viên và Sinh viên có thể đăng ký tài khoản mới.

## 🔑 Phân quyền mặc định

| Vai trò   | Quyền truy cập                                |
| --------- | --------------------------------------------- |
| Admin     | Toàn quyền quản trị                           |
| Giáo viên | Quản lý khóa học, tài liệu, thông báo         |
| Sinh viên | Đăng ký khóa học, tải tài liệu, xem thông báo |

## 👨‍💻 Thành viên thực hiện

- [Tên sinh viên 1]
- [Tên sinh viên 2]
- [Tên sinh viên 3]

## 👩‍🏫 Giảng viên hướng dẫn

- [Tên giảng viên]

## 📌 Giấy phép

Đây là dự án phục vụ mục đích học tập, không sử dụng cho mục đích thương mại.


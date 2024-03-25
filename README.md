## Tổng quan 
- Đồ án Chuyên ngành Công nghệ phần mềm
  - Tên đề tài: Website Hỗ trợ tìm thuê phòng trọ
  - Mô tả:
      - Client-side: đăng tin, quản lý thông tin bài đăng và thông tin cá nhân, thanh toán gói tin hiển thị, etc.
      - Server-side: quản lý thông tin toàn hệ thống, xét duyệt bài đăng, etc.
  - Giảng viên hướng dẫn: Nguyễn Trí Định
 - Thành viên:
   -   [Phan Anh Thăng](https://github.com/thanqphan) - 2011069025
   - [Vũ Lê Anh Thi](https://github.com/Anhthi0210) - 2011061865
   - [Huỳnh Thị Trúc Ngân](https://github.com/thanqphan) - 2011064432
   - [Vũ Phúc Lộc](https://github.com/thanqphan) - 2011060583
 - Công nghệ:
   - `C#` `JavaScript` `ASP .NET Core 6.0 MVC` `MS SQL`
## Mục lục
- [Tổng quan](#tong-quan)
- [Mục lục](#muc-luc)
- [1. Cơ sở lý thuyết](#co-so=li-thuyet)
  - [1.1 Ngôn ngữ sử dụng](#ngon-ngu-su-dung)
  - [1.2 Công cụ sử dụng](#cong-cu-su-dung)
  - [1.3 Hệ quản trị cơ sở dữ liệu](#he-quan-tri-csdl)
  - [1.4 Mô hình - kỹ thuật](#mo-hinh)
- [2. Phân tích thiết kế hệ thống](#phan-tich-thiet-ke)
  - [2.1 Phân tích hệ thống](#phan-tich-he-thong)
  - [2.2 Usecase Diagram](#use-case)
  - [2.3 Usecase Diagram đặc tả](#use-case-spec)
  - [2.4 ERD Diagram](#erd-diagram)
  - [2.5 Activity Diagram](#act-diagram)
  - [2.6 Sequence Diagram](#seq-diagram)
  - [2.7 Kiến trúc hệ thống](#kien-truc-he-thong)
- [3. Kết quả thực nghiệm](#ket-qua-thuc-nghiem)
  -   [3.1 Giao diện Người dùng](#giao-dien-client)
  -   [3.2 Giao diện Quản trị](#giao-dien-admin)
- [4. Kết luận - kiến nghị](#ket-luan-kien-nghi)
  -   [4.1 Kêt luận](#ket-luan)
  -   [4.2 Kiến nghị](#kien-nghi)
# 1. Cơ sở lý thuyết
## 1.1 Ngôn ngữ sử dụng
- `ASP.NET Core MVC` là một  framework “nhẹ”, opensource, giúp tối ưu hóa hiệu năng của ứng dụng với  ASP.NET Core .
-	`ASP.NET Core MVC` cung cấp các tính năng dựa trên mô hình xây dựng website động cho phép phân chia rõ ràng các khối lệnh. Nó cung cấp cho bạn toàn quyền kiểm soát đánh dấu, hỗ trợ phát triển  với TDD-friendly và sử dụng các tiêu chuẩn web mới nhất.
## 1.2 Công cụ sử dụng
- Microsof Visual Studio 2020
- SQL Server Management Studio 2019
- Github 
## 1.3 Hệ quản trị cơ sở dữ liệu
-	`SQL Server 2019` là bộ phận quản lý cơ sở dữ liệu, được xây dựng dựa trên khái niệm trí tuệ nhân tạo nhằm tạo điều kiện thuận lợi, cải tiến dịch vụ cơ sở dữ liệu, bảo mật và giảm bớt các khó khăn gặp phải khi phát triển các ứng dụng và lưu trữ dữ liệu. 
## 1.4 Mô hình - kỹ thuật
- Mô hình MVC (Model – View – Controller)
  - Model: đại diện cho dữ liệu và xử lý logic, thực hiện các tác vụ như lấy và lưu trữ dữ liệu, kiểm tra tính hợp lệ của dữ liệu và thực hiện các tính toán logic.
  -	View: đại diện cho người dùng, hiển thị thông tin cho người dùng và tương tác với họ. Nó cập nhật thông tin từ Model và hiển thị nó trên giao diện.
  -	Controller: đóng vai trò là trung gian giữa Model và View, điều khiển luồng dữ liệu và xử lý các sự kiện và yêu cầu của người dùng. Nó là nơi xử lý các yêu cầu và cập nhật từ Model sau khi dữ liệu đã được xử lý.
<p align="center">
    <img src="https://static-xf1.vietnix.vn/wp-content/uploads/2022/03/cac-thanh-phan-cua-mvc.webp" width="384" alt="Themer logo" />
</p>

# 2. Phân tích thiết kế hệ thống
## 2.1 Phân tích hệ thống
- Chức năng của hệ thống
  -	Hệ thống quản trị:
    - Quản lý tài khoản – nhân viên.
    - Quản lý tài khoản – người dùng.
    - 	Quản lý thông báo.
    - 	Quản lý Tin tức – Danh mục tin tức, bài viết.
    - 	Quản lý danh sách phòng trọ.
    - 	Quản lý thông tin người dùng -  người thuê và cho thuê.
    - 	Quản lý gói tin – nạp tiền
    -  Thống kê – báo cáo.
  -	Hệ thống hiển thị phía người dùng:
    -  	Hiển thị danh mục phòng trọ - theo tùy chọn (giá – quận – loại phòng) .
    -  	Hiển thị thông tin phòng trọ: thông tin người cho thuê – thông tin loại phòng thuê.
    -  	Hiển thị bài biết, tin tức.
    -  	Hiển thị thông tin về công ty: cách thức liên lạc, mạng xã hội, địa chỉ thực, email, …
    -  	Cho phép người dùng đăng ký, đăng nhập, thay đổi thông tin tài khoản.
    -  	Cho phép người dùng thuê hoạt cho thuê phòng trọ, thay đổi thông tin bài đăng, cách thức liên lạc,… 
    - Cho phép người dùng mua gói khi đăng bài, nạp tiền qua hình thức chuyển khoản qua ví điện tử.
- Tác nhân của hệ thống
  -	Admin - thực hiện chức năng quản trị hệ thống (nhân viên hỗ trợ): tài khoản, người dùng, phân quyền, giao diện, nạp gói – số dư.
  -	Nhân viên hỗ trợ: kiểm duyệt bài đăng, tương tác với người dùng, hỗ trợ khi người dùng đặt câu hỏi.
  -	Người dùng: đăng ký – đăng nhập tài khoản, tìm kiếm phòng trọ, cho thuê phòng trọ, nap gói – đăng tin,…

## 2.2 Usecase diagram
## 2.3 Đặc tả Usecase diagram
## 2.4 ERD diagram
## 2.5 Activity diagram
## 2.6 Sequence diagram
## 2.7 Kiến trúc hệ thống
# 3. Kết quả thực nghiệm
## 3.1 Giao diện người dùng
## 3.2 Giao diện quản trị
# 4. Kết luận - kiến nghị
## 4.1 Kết luận 
## 4.2 Kiến nghị

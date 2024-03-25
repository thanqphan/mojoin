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
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/ecf1ee11-6fde-4932-a7f9-91371dd75e27" width="600" alt="Themer logo" />
</p>
## 2.3 Đặc tả Usecase diagram
- Use case Truy cập vào website
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/faf99a08-7969-4e38-b83f-d8147061d793" width="600" alt="Themer logo" />
</p>
- Use case Tìm kiếm	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/69f395fd-69f8-4ee1-9080-b3da30962359" width="600" alt="Themer logo" />
</p>
- Use case Đăng ký tài khoản	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/3634c183-b71e-4de7-8fb9-6f464594ea99" width="600" alt="Themer logo" />
</p>
- Use case Đăng nhập	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/06ee20a6-cc9a-4bf9-a090-95c2417c408f" width="600" alt="Themer logo" />
</p>
- Use case Đăng bài
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/d9aac24e-1428-4dc4-ad4d-c9b67f950d41" width="600" alt="Themer logo" />
</p>
- Use case Quản lý nạp tiền	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/1ff15a14-ec6b-4196-baf3-eeaa859c0911" width="600" alt="Themer logo" />
</p>
- Use case Quản lý bài đăng – Cá nhân	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/9898ab8c-676b-4f2b-9d8b-000a61719c75" width="600" alt="Themer logo" />
</p>
- Use case Quản lý tài khoản – Cá nhân	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/e15216de-76f6-48e5-9365-f25a04f9e806" width="600" alt="Themer logo" />
</p>
- Use case Quản lý tài khoản – Admin	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/29aa88a7-8753-41a2-9446-086ec2c49bec" width="600" alt="Themer logo" />
</p>
- Use case Quản lý bài đăng – Admin	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/79fe8372-3e0f-4dbb-8bc3-75fec0e679f6" width="600" alt="Themer logo" />
</p>
## 2.4 ERD diagram
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/d6a99380-1651-40cc-a253-ca45f59a209f" width="600" alt="Themer logo" />
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/5f4d546d-b99d-4ec7-b5aa-b8cc590f60e6" width="600" alt="Themer logo" />
</p>

- [Link ERD diagram 😸](https://drive.google.com/file/d/19FWti0rIkf_gSi38tWQG6XQbeQXby7oy/view?usp=drive_link)
## 2.5 Activity diagram
- Activity diagram đăng kí	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/a503168f-3bb0-4497-abb6-a89e9744cdd7" width="600" alt="Themer logo" />
</p>
- Activity diagram đăng nhập	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/cfabf02c-4fa5-4c38-a177-f824265eed50" width="600" alt="Themer logo" />
</p>
- Activity diagram lưu tin vào danh sách yêu thích	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/94a37633-754c-4e65-b80e-7c448e9364dc" width="600" alt="Themer logo" />
</p>
- Activity diagram bỏ lưu tin 
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/4bd2a530-6c1f-47f9-91a0-04d3bed9f91f" width="600" alt="Themer logo" />
</p>
- Activity diagram duyệt bài
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/57ac99c9-b8db-43db-a04e-3d2ffa10fe39" width="600" alt="Themer logo" />
</p>
- Activity diagram đăng tin	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/f24c256b-a6d9-44ee-a149-e2ac97c4b1eb" width="600" alt="Themer logo" />
</p>
- Activity diagram nạp tiền	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/35141278-9daf-434d-8c73-d9d0e95cd53d" width="600" alt="Themer logo" />
</p>
- Activity diagram mua gói	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/cfa08601-0667-4bed-bab3-6d4b91286070" width="600" alt="Themer logo" />
</p>

## 2.6 Sequence diagram
- Squence Diagram Đăng kí
- <p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/bb6bbca1-b4a0-4304-a44b-18b3bd0dd6cb" width="600" alt="Themer logo" />
</p>
- Squence Diagram Đăng nhập
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/7c7f8218-c88a-45af-85c7-ee6d7f9fd5c3" width="600" alt="Themer logo" />
</p>
- Squence Diagram Đăng bài	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/57d8c1aa-4bd8-4c03-83d8-459371d8935f" width="600" alt="Themer logo" />
</p>
- Squence Diagram Duyệt bài đăng	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/9c394486-c536-475f-b57f-b18451562c23" width="600" alt="Themer logo" />
</p>
- Squence Diagram Tìm kiếm	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/b8504fe9-42d1-4695-a624-40b3b23a4419" width="600" alt="Themer logo" />
</p>
- Squence Diagram Yêu thích	
<p align="center">
    <img src="https://github.com/thanqphan/mojoin/assets/118738430/53e9ff9d-1e61-40ba-bd1e-d4a007b1d726" width="600" alt="Themer logo" />
</p>
## 2.7 Kiến trúc hệ thống
- Mô hình vật lý	

- Mô hình chi tiết các bảng	

# 3. Kết quả thực nghiệm
## 3.1 Giao diện người dùng
## 3.2 Giao diện quản trị
# 4. Kết luận - kiến nghị
## 4.1 Kết luận 
## 4.2 Kiến nghị

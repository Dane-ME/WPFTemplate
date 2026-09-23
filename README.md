# WPF Clean Architecture MVVM Navigation Template

Template dự án WPF chuẩn MVVM kết hợp Clean Architecture, tích hợp sẵn hệ thống Navigation tối ưu hiệu năng (LRU Cache, Back-stack, Hỗ trợ Async & CancellationToken) và Microsoft Dependency Injection.

---

## 1. Mục Đích

- Cung cấp một boilerplate chuẩn mực để khởi tạo nhanh các ứng dụng WPF Enterprise / Desktop.
- Giải quyết triệt để các vấn đề thường gặp trong WPF navigation:
  - Giật lag do khởi tạo View/ViewModel lặp lại.
  - Rò rỉ bộ nhớ do không quản lý vòng đời ViewModel.
  - Race condition khi người dùng chuyển trang liên tục trong lúc đang load dữ liệu async.
  - Phụ thuộc chéo giữa View và ViewModel.

---

## 2. Ưu Điểm

- **Tách bạch tầng theo Clean Architecture:**
  - `Domain`: Chứa các quy tắc cốt lõi và Result Pattern không phụ thuộc bên ngoài.
  - `Application`: Định nghĩa Use Cases, Business Logic, Interfaces.
  - `Infrastructure`: Thực thi các dịch vụ ngoại vi, truy xuất dữ liệu, API.
  - `Presentation (WPF)`: Giao diện thuần MVVM, View tự phân giải qua DataTemplate mapping dựa trên kiểu ViewModel.
- **Cơ chế Navigation mạnh mẽ:**
  - **LRU Cache & Lazy Factory:** Chỉ khởi tạo ViewModel khi cần, tự giải phóng instance cũ nhất khi vượt ngưỡng cache (`maxCacheSize`).
  - **Back-Stack History:** Hỗ trợ điều hướng lùi (`GoBack()`, `GoBackAsync()`).
  - **Async Navigation & Cancellation Token:** Tự động hủy task load ngầm trước đó nếu người dùng chuyển trang nhanh liên tục.
  - **Lifecycle Hook (`INavigationAware`):** Hỗ trợ `OnNavigatedTo`, `OnNavigatedToAsync`, `CanNavigateFrom`, `CanNavigateFromAsync`.
- **DI Composition Root chuẩn:** Cấu hình toàn bộ trong `App.xaml.cs`.

---

## 3. Giới Hạn

- Chưa tích hợp sẵn Dialog Service / Popup Modal dạng đa cửa sổ (tập trung vào ContentControl Navigation trong Single-Window).
- Chưa tích hợp EventAggregator / Messenger cho các ViewModel không cùng phân nhánh giao tiếp trực tiếp.
- `ContentControl` mặc định không có hiệu ứng chuyển cảnh (Transitions/Animations). Nếu cần hiệu ứng mượt mà, cần bổ sung Storyboard/VisualStateManager.

---

## 4. Cách Sử Dụng & Cài Đặt

### Cách 1: Cài đặt trực tiếp từ mã nguồn vào Visual Studio / .NET CLI

Từ thư mục gốc chứa file `.template.config`:

```bash
dotnet new install .
```

Sau khi cài đặt:
- **Từ dòng lệnh:**
  ```bash
  dotnet new wpf-clean-nav -n MyAwesomeApp
  ```
- **Từ Visual Studio:**
  1. Mở Visual Studio > **Create a new project**.
  2. Tìm kiếm `wpf-clean-nav` hoặc `WPF Clean Architecture`.
  3. Đặt tên và bấm **Create**.

### Cách 2: Đóng gói thành `.nupkg` và phân phối / Push GitHub

1. Đóng gói template thành NuGet Package:
   ```bash
   dotnet pack package.csproj -o ./dist
   ```
2. Cài đặt từ package vừa tạo:
   ```bash
   dotnet new install ./dist/WpfCleanArchNavTemplate.1.0.0.nupkg
   ```
3. Push package lên GitHub Packages hoặc NuGet:
   ```bash
   dotnet nuget push ./dist/*.nupkg --api-key <YOUR_KEY> --source https://api.nuget.org/v3/index.json
   ```

### Gỡ cài đặt Template:
```bash
dotnet new uninstall WpfCleanArchNavTemplate
```

# WPF Clean Architecture MVVM Navigation Template

A production-ready WPF project template implementing MVVM and Clean Architecture principles, featuring a high-performance Navigation Service (LRU Cache, Back-stack, Async & CancellationToken support) and Microsoft Dependency Injection.

---

## 1. Purpose

- Provide a standardized boilerplate for rapidly scaffolding enterprise and modern desktop WPF applications.
- Completely resolve common issues in WPF navigation:
  - UI freezes/lag caused by repetitive View/ViewModel instantiation.
  - Memory leaks due to unmanaged ViewModel lifecycles.
  - Race conditions caused by rapid page-switching while background async operations are running.
  - Tight coupling between Views and ViewModels.

---

## 2. Key Features

- **Layer Separation via Clean Architecture:**
  - `Domain`: Core enterprise business rules and external-dependency-free Result Pattern.
  - `Application`: Use Cases, business logic orchestration, and interface abstractions.
  - `Infrastructure`: External services, data persistence, and API clients.
  - `Presentation (WPF)`: Pure MVVM presentation layer; Views are resolved automatically via `DataTemplate` mapping based on the ViewModel type.
- **Robust Navigation Engine:**
  - **LRU Cache & Lazy Factory:** ViewModels are instantiated on-demand and automatically evicted/disposed when exceeding the cache limit (`maxCacheSize`).
  - **Back-Stack History:** Full backward navigation support (`GoBack()`, `GoBackAsync()`).
  - **Async Navigation & Cancellation Token:** Automatically cancels pending background data-loading tasks if the user navigates away rapidly.
  - **Lifecycle Hooks (`INavigationAware`):** First-class lifecycle methods (`OnNavigatedTo`, `OnNavigatedToAsync`, `CanNavigateFrom`, `CanNavigateFromAsync`).
- **Standard DI Composition Root:** Centrally configured within `App.xaml.cs`.

---

## 3. Limitations

- Does not include a multi-window Dialog / Modal Popup service out of the box (focuses on single-window `ContentControl` navigation).
- Does not bundle an `EventAggregator` / `Messenger` for cross-branch ViewModel communication.
- `ContentControl` does not include default page transitions/animations. Add Storyboards or VisualStateManagers if transitions are required.

---

## 4. Getting Started: Installation & Scaffolding

### Step 1: Install the Template

Open your terminal (PowerShell, Command Prompt, or Visual Studio Developer Terminal) and run:

```bash
# Install directly from NuGet
dotnet new install WpfCleanArchNavTemplate
```

> **Note:** If you have the template source code locally, you can install it directly via:
> ```bash
> dotnet new install .
> ```

---

### Step 2: Create a New Project

#### Option A: Using the .NET CLI (Command Line)

Scaffold a complete solution with your custom application name (e.g., `MyAwesomeApp`):

```bash
# 1. Create a new project from the template
dotnet new wpf-clean-nav -n MyAwesomeApp

# 2. Navigate into the newly created directory
cd MyAwesomeApp

# 3. Run the application
dotnet run --project MyAwesomeApp.wpf/MyAwesomeApp.wpf.csproj
```

*The CLI automatically generates and renames all projects to match your solution name:*
- `MyAwesomeApp.slnx`
- `MyAwesomeApp.Domain`
- `MyAwesomeApp.Application`
- `MyAwesomeApp.Infrastructure`
- `MyAwesomeApp.wpf`

---

#### Option B: Using Visual Studio

1. Open Visual Studio and click **Create a new project**.
2. In the top search bar, enter: `wpf-clean-nav` or `WPF Clean Architecture`.
3. Select **WPF Clean Architecture MVVM Navigation Template** and click **Next**.
4. Configure your **Project name** and storage location, then click **Create**.
5. Visual Studio will generate the entire solution ready to run with `F5`.

---

### Template Management

- **List installed templates:**
  ```bash
  dotnet new list
  ```
- **Update to the latest version:**
  ```bash
  dotnet new update
  ```
- **Uninstall the template:**
  ```bash
  dotnet new uninstall WpfCleanArchNavTemplate
  ```

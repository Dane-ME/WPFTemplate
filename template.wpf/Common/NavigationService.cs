namespace template.wpf.Common;

public class NavigationService : INavigationService
{
    private readonly Func<Type, BaseViewModel> _viewModelFactory;
    private readonly Stack<BaseViewModel> _backStack = new();
    private readonly LinkedList<Type> _lruOrder = new();
    private readonly Dictionary<Type, BaseViewModel> _cache = new();
    private readonly int _maxCacheSize;
    private BaseViewModel? _currentView;
    private CancellationTokenSource? _activeNavigationCts;

    public BaseViewModel? CurrentView
    {
        get => _currentView;
        private set
        {
            if (_currentView != value)
            {
                _currentView = value;
                CurrentViewChanged?.Invoke();
            }
        }
    }

    public bool CanGoBack => _backStack.Count > 0;

    public event Action? CurrentViewChanged;

    public NavigationService(Func<Type, BaseViewModel> viewModelFactory, int maxCacheSize = 5)
    {
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
        _maxCacheSize = maxCacheSize;
    }

    public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : BaseViewModel
    {
        CancelActiveNavigation();

        if (_currentView is INavigationAware currentAware && !currentAware.CanNavigateFrom())
        {
            return;
        }

        var targetType = typeof(TViewModel);

        if (_currentView is not null && _currentView.GetType() == targetType)
        {
            if (_currentView is INavigationAware selfAware)
            {
                selfAware.OnNavigatedTo(parameter);
            }
            return;
        }

        var viewModel = GetOrCreateViewModel(targetType);

        if (_currentView is not null)
        {
            _backStack.Push(_currentView);
        }

        CurrentView = viewModel;

        if (viewModel is INavigationAware nextAware)
        {
            nextAware.OnNavigatedTo(parameter);
        }
    }

    public async Task NavigateToAsync<TViewModel>(object? parameter = null, CancellationToken cancellationToken = default) where TViewModel : BaseViewModel
    {
        CancelActiveNavigation();

        _activeNavigationCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var linkedToken = _activeNavigationCts.Token;

        if (_currentView is INavigationAware currentAware)
        {
            var canLeave = await currentAware.CanNavigateFromAsync().ConfigureAwait(true);
            if (!canLeave || linkedToken.IsCancellationRequested)
            {
                return;
            }
        }

        var targetType = typeof(TViewModel);

        if (_currentView is not null && _currentView.GetType() == targetType)
        {
            if (_currentView is INavigationAware selfAware)
            {
                selfAware.OnNavigatedTo(parameter);
                await selfAware.OnNavigatedToAsync(parameter, linkedToken).ConfigureAwait(true);
            }
            return;
        }

        var viewModel = GetOrCreateViewModel(targetType);

        if (linkedToken.IsCancellationRequested)
        {
            return;
        }

        if (_currentView is not null)
        {
            _backStack.Push(_currentView);
        }

        CurrentView = viewModel;

        if (viewModel is INavigationAware nextAware)
        {
            nextAware.OnNavigatedTo(parameter);
            await nextAware.OnNavigatedToAsync(parameter, linkedToken).ConfigureAwait(true);
        }
    }

    public void GoBack()
    {
        CancelActiveNavigation();

        if (!CanGoBack) return;

        if (_currentView is INavigationAware currentAware && !currentAware.CanNavigateFrom())
        {
            return;
        }

        var previousViewModel = _backStack.Pop();
        CurrentView = previousViewModel;

        if (previousViewModel is INavigationAware prevAware)
        {
            prevAware.OnNavigatedTo(null);
        }
    }

    public async Task GoBackAsync(CancellationToken cancellationToken = default)
    {
        CancelActiveNavigation();

        if (!CanGoBack) return;

        _activeNavigationCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var linkedToken = _activeNavigationCts.Token;

        if (_currentView is INavigationAware currentAware)
        {
            var canLeave = await currentAware.CanNavigateFromAsync().ConfigureAwait(true);
            if (!canLeave || linkedToken.IsCancellationRequested)
            {
                return;
            }
        }

        var previousViewModel = _backStack.Pop();
        CurrentView = previousViewModel;

        if (previousViewModel is INavigationAware prevAware)
        {
            prevAware.OnNavigatedTo(null);
            await prevAware.OnNavigatedToAsync(null, linkedToken).ConfigureAwait(true);
        }
    }

    public void ClearCache()
    {
        CancelActiveNavigation();

        foreach (var vm in _cache.Values)
        {
            if (vm is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        _cache.Clear();
        _lruOrder.Clear();
    }

    private void CancelActiveNavigation()
    {
        if (_activeNavigationCts is not null)
        {
            _activeNavigationCts.Cancel();
            _activeNavigationCts.Dispose();
            _activeNavigationCts = null;
        }
    }

    private BaseViewModel GetOrCreateViewModel(Type type)
    {
        if (_cache.TryGetValue(type, out var cached))
        {
            _lruOrder.Remove(type);
            _lruOrder.AddFirst(type);
            return cached;
        }

        var instance = _viewModelFactory(type);

        if (_maxCacheSize > 0)
        {
            if (_cache.Count >= _maxCacheSize && _lruOrder.Last is not null)
            {
                var oldest = _lruOrder.Last.Value;
                _lruOrder.RemoveLast();

                if (_cache.Remove(oldest, out var evicted) && evicted is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _cache[type] = instance;
            _lruOrder.AddFirst(type);
        }

        return instance;
    }
}

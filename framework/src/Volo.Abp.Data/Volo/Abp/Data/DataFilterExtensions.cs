using System;

namespace Volo.Abp.Data;

public static class DataFilterExtensions
{
    private sealed class CompositeDisposable : IDisposable
    {
        private readonly IDisposable[] _disposables;
        private bool _disposed;

        public CompositeDisposable(IDisposable[] disposables)
        {
            _disposables = disposables;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
        }
    }

    public static IDisposable Disable<T1, T2>(this IDataFilter filter)
        where T1 : class
        where T2 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Disable<T1>(),
            filter.Disable<T2>()
        });
    }

    public static IDisposable Disable<T1, T2, T3>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Disable<T1>(),
            filter.Disable<T2>(),
            filter.Disable<T3>()
        });
    }

    public static IDisposable Disable<T1, T2, T3, T4>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Disable<T1>(),
            filter.Disable<T2>(),
            filter.Disable<T3>(),
            filter.Disable<T4>()
        });
    }

    public static IDisposable Disable<T1, T2, T3, T4, T5>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Disable<T1>(),
            filter.Disable<T2>(),
            filter.Disable<T3>(),
            filter.Disable<T4>(),
            filter.Disable<T5>()
        });
    }

    public static IDisposable Disable<T1, T2, T3, T4, T5, T6>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Disable<T1>(),
            filter.Disable<T2>(),
            filter.Disable<T3>(),
            filter.Disable<T4>(),
            filter.Disable<T5>(),
            filter.Disable<T6>()
        });
    }

    public static IDisposable Disable<T1, T2, T3, T4, T5, T6, T7>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Disable<T1>(),
            filter.Disable<T2>(),
            filter.Disable<T3>(),
            filter.Disable<T4>(),
            filter.Disable<T5>(),
            filter.Disable<T6>(),
            filter.Disable<T7>()
        });
    }

    public static IDisposable Disable<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Disable<T1>(),
            filter.Disable<T2>(),
            filter.Disable<T3>(),
            filter.Disable<T4>(),
            filter.Disable<T5>(),
            filter.Disable<T6>(),
            filter.Disable<T7>(),
            filter.Disable<T8>()
        });
    }

    public static IDisposable Enable<T1, T2>(this IDataFilter filter)
        where T1 : class
        where T2 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Enable<T1>(),
            filter.Enable<T2>()
        });
    }

    public static IDisposable Enable<T1, T2, T3>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Enable<T1>(),
            filter.Enable<T2>(),
            filter.Enable<T3>()
        });
    }

    public static IDisposable Enable<T1, T2, T3, T4>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Enable<T1>(),
            filter.Enable<T2>(),
            filter.Enable<T3>(),
            filter.Enable<T4>()
        });
    }

    public static IDisposable Enable<T1, T2, T3, T4, T5>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Enable<T1>(),
            filter.Enable<T2>(),
            filter.Enable<T3>(),
            filter.Enable<T4>(),
            filter.Enable<T5>()
        });
    }

    public static IDisposable Enable<T1, T2, T3, T4, T5, T6>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Enable<T1>(),
            filter.Enable<T2>(),
            filter.Enable<T3>(),
            filter.Enable<T4>(),
            filter.Enable<T5>(),
            filter.Enable<T6>()
        });
    }

    public static IDisposable Enable<T1, T2, T3, T4, T5, T6, T7>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Enable<T1>(),
            filter.Enable<T2>(),
            filter.Enable<T3>(),
            filter.Enable<T4>(),
            filter.Enable<T5>(),
            filter.Enable<T6>(),
            filter.Enable<T7>()
        });
    }

    public static IDisposable Enable<T1, T2, T3, T4, T5, T6, T7, T8>(this IDataFilter filter)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
    {
        return new CompositeDisposable(new[]
        {
            filter.Enable<T1>(),
            filter.Enable<T2>(),
            filter.Enable<T3>(),
            filter.Enable<T4>(),
            filter.Enable<T5>(),
            filter.Enable<T6>(),
            filter.Enable<T7>(),
            filter.Enable<T8>()
        });
    }
}

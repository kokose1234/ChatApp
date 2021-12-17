using System.Collections.Concurrent;

namespace ChatApp.Common.Tools;

public class ObjectPool<T> where T : class, new()
{
    private readonly ConcurrentBag<T> _objects;
    private readonly Func<T> _generator;

    public ObjectPool(Func<T> generator)
    {
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        _objects = new ConcurrentBag<T>();

        for (var i = 0; i < 30; i++)
        {
            _objects.Add(_generator());
        }
    }

    public T Get() => _objects.TryTake(out var item) ? item : _generator();

    public void Return(T item) => _objects.Add(item);
}
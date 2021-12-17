namespace ChatApp.Client.Data;

internal sealed class WaitableResult<T>
{
    private readonly ManualResetEvent _event;

    internal T Value { get; private set; }

    internal WaitableResult() => _event = new ManualResetEvent(false);

    internal void Wait() => _event.WaitOne();

    internal void Set(T value)
    {
        Value = value;
        _event.Set();
    }
}
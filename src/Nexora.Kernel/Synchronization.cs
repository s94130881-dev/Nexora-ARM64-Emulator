namespace Nexora.Kernel;

public sealed class KernelMutex
{
    private readonly object _lock = new();

    private bool _locked;

    public bool TryLock()
    {
        lock (_lock)
        {
            if (_locked)
                return false;

            _locked = true;

            return true;
        }
    }

    public void Unlock()
    {
        lock (_lock)
        {
            _locked = false;
        }
    }

    public bool IsLocked
    {
        get
        {
            lock (_lock)
            {
                return _locked;
            }
        }
    }
}

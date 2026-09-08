namespace Nexora.Kernel;

public sealed class Scheduler
{
    private readonly Queue<KernelThread> _readyQueue = new();

    public KernelThread? CurrentThread { get; private set; }

    public void AddThread(KernelThread thread)
    {
        if (thread.State == ThreadState.Terminated)
            return;

        thread.Ready();

        _readyQueue.Enqueue(thread);
    }

    public KernelThread? Schedule()
    {
        if (_readyQueue.Count == 0)
        {
            CurrentThread = null;
            return null;
        }

        if (CurrentThread != null &&
            CurrentThread.State == ThreadState.Running)
        {
            CurrentThread.Ready();
            _readyQueue.Enqueue(CurrentThread);
        }

        CurrentThread = _readyQueue.Dequeue();

        CurrentThread.Run();

        return CurrentThread;
    }

    public void StopCurrent()
    {
        if (CurrentThread == null)
            return;

        CurrentThread.Ready();
        _readyQueue.Enqueue(CurrentThread);

        CurrentThread = null;
    }
}

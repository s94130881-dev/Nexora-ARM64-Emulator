namespace Nexora.Kernel;

public sealed class KernelProcess
{
    private readonly List<KernelThread> _threads = new();

    public int Id { get; }

    public string Name { get; }

    public ulong EntryPoint { get; }

    public IReadOnlyList<KernelThread> Threads =>
        _threads;

    public KernelProcess(
        int id,
        string name,
        ulong entryPoint)
    {
        Id = id;
        Name = name;
        EntryPoint = entryPoint;
    }

    public KernelThread CreateThread(
        ulong stackPointer)
    {
        int threadId = _threads.Count + 1;

        var thread = new KernelThread(
            threadId,
            EntryPoint,
            stackPointer);

        thread.Ready();

        _threads.Add(thread);

        return thread;
    }

    public void Terminate()
    {
        foreach (var thread in _threads)
            thread.Terminate();
    }
}

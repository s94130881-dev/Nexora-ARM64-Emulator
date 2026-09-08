using Nexora.Memory;

namespace Nexora.Kernel;

public sealed class Kernel
{
    private readonly List<KernelProcess> _processes = new();

    private int _nextProcessId = 1;

    public MemoryManager Memory { get; }

    public Scheduler Scheduler { get; }

    public IReadOnlyList<KernelProcess> Processes =>
        _processes;

    public Kernel(MemoryManager memory)
    {
        Memory = memory;
        Scheduler = new Scheduler();
    }

    public KernelProcess CreateProcess(
        string name,
        ulong entryPoint)
    {
        var process = new KernelProcess(
            _nextProcessId++,
            name,
            entryPoint);

        _processes.Add(process);

        return process;
    }

    public KernelThread CreateMainThread(
        KernelProcess process,
        ulong stackPointer)
    {
        var thread =
            process.CreateThread(stackPointer);

        Scheduler.AddThread(thread);

        return thread;
    }

    public void Schedule()
    {
        Scheduler.Schedule();
    }

    public void Shutdown()
    {
        foreach (var process in _processes)
            process.Terminate();

        _processes.Clear();
    }
}

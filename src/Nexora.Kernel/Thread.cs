using Nexora.CPU;

namespace Nexora.Kernel;

public enum ThreadState
{
    Created,
    Ready,
    Running,
    Waiting,
    Terminated
}

public sealed class KernelThread
{
    public int Id { get; }

    public CpuState CpuState { get; }

    public ThreadState State { get; private set; }

    public ulong EntryPoint { get; }

    public ulong StackPointer { get; set; }

    public KernelThread(
        int id,
        ulong entryPoint,
        ulong stackPointer)
    {
        Id = id;
        EntryPoint = entryPoint;
        StackPointer = stackPointer;

        CpuState = new CpuState();

        CpuState.Reset(entryPoint);
        CpuState.SP = stackPointer;

        State = ThreadState.Created;
    }

    public void Ready()
    {
        if (State == ThreadState.Terminated)
            return;

        State = ThreadState.Ready;
    }

    public void Run()
    {
        if (State == ThreadState.Terminated)
            return;

        State = ThreadState.Running;
    }

    public void Wait()
    {
        if (State == ThreadState.Terminated)
            return;

        State = ThreadState.Waiting;
    }

    public void Terminate()
    {
        State = ThreadState.Terminated;
    }
}

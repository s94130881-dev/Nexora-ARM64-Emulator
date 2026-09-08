using Nexora.Memory;

namespace Nexora.CPU;

public sealed class Cpu
{
    public CpuState State { get; }

    public A64Interpreter Interpreter { get; }

    public Cpu(MemoryManager memory)
    {
        State = new CpuState();
        Interpreter = new A64Interpreter(State, memory);
    }

    public void Reset(ulong entryPoint)
    {
        State.Reset(entryPoint);
    }

    public void Step()
    {
        Interpreter.Step();
    }
}

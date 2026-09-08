using Nexora.Memory;

namespace Nexora.CPU;

public sealed class A64Interpreter
{
    private readonly CpuState _state;
    private readonly MemoryManager _memory;

    public A64Interpreter(
        CpuState state,
        MemoryManager memory)
    {
        _state = state;
        _memory = memory;
    }

    public void Step()
    {
        ulong pc = _state.PC;

        uint raw = _memory.ReadUInt32(pc);

        var instruction = A64Decoder.Decode(raw);

        _state.PC = pc + 4;

        Execute(instruction);
    }

    private void Execute(A64Instruction instruction)
    {
        uint raw = instruction.Raw;

        if (A64Decoder.IsBranch(raw))
        {
            Instructions.BranchInstructions.Execute(
                _state,
                raw);

            return;
        }

        if (A64Decoder.IsAddSubImmediate(raw))
        {
            Instructions.DataProcessing.ExecuteAddSub(
                _state,
                raw);

            return;
        }

        throw new NotImplementedException(
            $"ARM64 instruction não implementada: {instruction}");
    }
}

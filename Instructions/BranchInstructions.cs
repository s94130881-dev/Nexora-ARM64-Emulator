namespace Nexora.CPU.Instructions;

public static class BranchInstructions
{
    public static void Execute(
        CpuState state,
        uint instruction)
    {
        int imm26 = (int)(instruction & 0x03FFFFFF);

        if ((imm26 & 0x02000000) != 0)
            imm26 |= unchecked((int)0xFC000000);

        long offset = (long)imm26 << 2;

        state.PC = unchecked(
            (ulong)((long)state.PC - 4 + offset));
    }
}

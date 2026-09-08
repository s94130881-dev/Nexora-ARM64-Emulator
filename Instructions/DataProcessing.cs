namespace Nexora.CPU.Instructions;

public static class DataProcessing
{
    public static void ExecuteAddSub(
        CpuState state,
        uint instruction)
    {
        bool subtract =
            (instruction & (1u << 30)) != 0;

        bool setFlags =
            (instruction & (1u << 29)) != 0;

        bool is64 =
            (instruction & (1u << 31)) != 0;

        int rd = (int)(instruction & 0x1F);
        int rn = (int)((instruction >> 5) & 0x1F);

        ulong immediate =
            (instruction >> 10) & 0xFFF;

        ulong left =
            state.GetRegister(rn);

        ulong result;

        if (subtract)
            result = left - immediate;
        else
            result = left + immediate;

        if (!is64)
            result &= 0xFFFFFFFF;

        state.SetRegister(rd, result);

        if (setFlags)
        {
            // NZCV será implementado posteriormente.
        }
    }
}

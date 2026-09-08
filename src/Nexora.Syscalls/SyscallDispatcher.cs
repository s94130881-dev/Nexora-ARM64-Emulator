using Nexora.CPU;
using Nexora.Kernel;
using Nexora.Memory;

namespace Nexora.Syscalls;

public sealed class SyscallDispatcher
{
    private readonly Kernel _kernel;
    private readonly MemoryManager _memory;

    public SyscallDispatcher(
        Kernel kernel,
        MemoryManager memory)
    {
        _kernel = kernel;
        _memory = memory;
    }

    public SyscallResult Dispatch(
        CpuState state,
        ulong number)
    {
        return number switch
        {
            SyscallTable.Exit =>
                HandleExit(state),

            SyscallTable.Read =>
                HandleRead(state),

            SyscallTable.Write =>
                HandleWrite(state),

            SyscallTable.Open =>
                HandleOpen(state),

            SyscallTable.Close =>
                HandleClose(state),

            SyscallTable.MapMemory =>
                HandleMapMemory(state),

            SyscallTable.UnmapMemory =>
                HandleUnmapMemory(state),

            SyscallTable.CreateThread =>
                HandleCreateThread(state),

            SyscallTable.Sleep =>
                HandleSleep(state),

            _ => Unknown(number)
        };
    }

    private SyscallResult HandleExit(
        CpuState state)
    {
        long code = unchecked(
            (long)state.GetRegister(0));

        return SyscallResult.Ok(code);
    }

    private SyscallResult HandleRead(
        CpuState state)
    {
        return SyscallResult.Error(-1);
    }

    private SyscallResult HandleWrite(
        CpuState state)
    {
        return SyscallResult.Error(-1);
    }

    private SyscallResult HandleOpen(
        CpuState state)
    {
        return SyscallResult.Error(-1);
    }

    private SyscallResult HandleClose(
        CpuState state)
    {
        return SyscallResult.Ok(0);
    }

    private SyscallResult HandleMapMemory(
        CpuState state)
    {
        ulong address = state.GetRegister(0);

        try
        {
            _memory.VirtualMemory.Map(
                address,
                writable: true,
                executable: false);

            return SyscallResult.Ok(
                unchecked((long)address));
        }
        catch
        {
            return SyscallResult.Error(-1);
        }
    }

    private SyscallResult HandleUnmapMemory(
        CpuState state)
    {
        ulong address = state.GetRegister(0);

        bool result =
            _memory.VirtualMemory.Unmap(address);

        return result
            ? SyscallResult.Ok(0)
            : SyscallResult.Error(-1);
    }

    private SyscallResult HandleCreateThread(
        CpuState state)
    {
        return SyscallResult.Error(-1);
    }

    private SyscallResult HandleSleep(
        CpuState state)
    {
        return SyscallResult.Ok(0);
    }

    private static SyscallResult Unknown(
        ulong number)
    {
        Console.WriteLine(
            $"[SYSCALL] Não implementada: " +
            $"{number} ({SyscallTable.GetName(number)})");

        return SyscallResult.Error(-38);
    }
}

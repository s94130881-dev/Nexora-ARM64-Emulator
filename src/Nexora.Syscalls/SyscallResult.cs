namespace Nexora.Syscalls;

public readonly struct SyscallResult
{
    public long Value { get; }

    public bool Success { get; }

    public SyscallResult(long value, bool success = true)
    {
        Value = value;
        Success = success;
    }

    public static SyscallResult Ok(long value)
    {
        return new SyscallResult(value, true);
    }

    public static SyscallResult Error(long error)
    {
        return new SyscallResult(error, false);
    }
}

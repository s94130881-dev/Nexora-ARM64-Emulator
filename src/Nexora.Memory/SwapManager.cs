namespace Nexora.Memory;

public sealed class SwapManager
{
    public ulong Size { get; }

    public ulong Used { get; private set; }

    public ulong Available =>
        Size - Used;

    public SwapManager(
        ulong size = MemoryConfiguration.SwapSize)
    {
        Size = size;
    }

    public bool Allocate(ulong bytes)
    {
        if (bytes > Available)
            return false;

        Used += bytes;

        return true;
    }

    public void Free(ulong bytes)
    {
        if (bytes > Used)
            Used = 0;
        else
            Used -= bytes;
    }
}

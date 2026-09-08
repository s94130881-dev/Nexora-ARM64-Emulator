namespace Nexora.Memory;

public static class MemoryConfiguration
{
    public const ulong PageSize = 4096;

    public const ulong PhysicalRam =
        4UL * 1024UL * 1024UL * 1024UL;

    public const ulong SwapSize =
        8UL * 1024UL * 1024UL * 1024UL;

    public const ulong TotalVirtualMemory =
        PhysicalRam + SwapSize;
}

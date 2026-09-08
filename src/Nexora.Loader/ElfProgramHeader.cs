namespace Nexora.Loader;

public sealed class ElfProgramHeader
{
    public const uint Load = 1;

    public uint Type { get; set; }

    public uint Flags { get; set; }

    public ulong Offset { get; set; }

    public ulong VirtualAddress { get; set; }

    public ulong PhysicalAddress { get; set; }

    public ulong FileSize { get; set; }

    public ulong MemorySize { get; set; }

    public ulong Alignment { get; set; }

    public bool IsLoadable =>
        Type == Load;
}

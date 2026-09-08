using Nexora.Memory;

namespace Nexora.Loader;

public sealed class ProgramLoader
{
    private readonly MemoryManager _memory;

    public ElfLoader Elf { get; }

    public ulong EntryPoint =>
        Elf.EntryPoint;

    public ProgramLoader(MemoryManager memory)
    {
        _memory = memory;
        Elf = new ElfLoader(memory);
    }

    public ulong Load(string path)
    {
        Elf.Load(path);

        return EntryPoint;
    }
}

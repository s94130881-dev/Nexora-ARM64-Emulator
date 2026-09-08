namespace Nexora.Memory;

public sealed class MemoryPage
{
    public ulong Address { get; }

    public bool IsMapped { get; private set; }

    public bool IsWritable { get; private set; }

    public bool IsExecutable { get; private set; }

    public MemoryPage(ulong address)
    {
        Address = address;
    }

    public void Map(
        bool writable = true,
        bool executable = false)
    {
        IsMapped = true;
        IsWritable = writable;
        IsExecutable = executable;
    }

    public void Unmap()
    {
        IsMapped = false;
        IsWritable = false;
        IsExecutable = false;
    }
}

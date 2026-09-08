namespace Nexora.Memory;

public sealed class VirtualMemory
{
    private readonly Dictionary<ulong, MemoryPage> _pages = new();

    public ulong Size { get; }

    public VirtualMemory(
        ulong size = MemoryConfiguration.TotalVirtualMemory)
    {
        Size = size;
    }

    public MemoryPage Map(
        ulong address,
        bool writable = true,
        bool executable = false)
    {
        address = AlignDown(address);

        ValidateAddress(address);

        if (_pages.ContainsKey(address))
            return _pages[address];

        var page = new MemoryPage(address);

        page.Map(writable, executable);

        _pages[address] = page;

        return page;
    }

    public bool Unmap(ulong address)
    {
        address = AlignDown(address);

        return _pages.Remove(address);
    }

    public bool IsMapped(ulong address)
    {
        address = AlignDown(address);

        return _pages.TryGetValue(address, out var page)
            && page.IsMapped;
    }

    private static ulong AlignDown(ulong address)
    {
        return address -
               (address % MemoryConfiguration.PageSize);
    }

    private void ValidateAddress(ulong address)
    {
        if (address >= Size)
            throw new ArgumentOutOfRangeException(
                nameof(address),
                $"Endereço 0x{address:X} fora da memória virtual.");
    }
}

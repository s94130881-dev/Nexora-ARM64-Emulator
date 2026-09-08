namespace Nexora.Memory;

public sealed class MemoryManager
{
    private readonly byte[] _memory;

    public VirtualMemory VirtualMemory { get; }

    public SwapManager Swap { get; }

    public ulong RamSize =>
        MemoryConfiguration.PhysicalRam;

    public ulong SwapSize =>
        MemoryConfiguration.SwapSize;

    public ulong TotalMemory =>
        MemoryConfiguration.TotalVirtualMemory;

    public MemoryManager()
    {
        /*
         * Não alocamos 4 GB aqui.
         *
         * O Android possui somente a RAM física disponível
         * no aparelho. A memória do guest será implementada
         * com paginação/backing posteriormente.
         *
         * Este buffer pequeno serve apenas para a primeira
         * implementação/testes do emulador.
         */
        _memory = new byte[16 * 1024 * 1024];

        VirtualMemory = new VirtualMemory();

        Swap = new SwapManager();
    }

    public byte ReadByte(ulong address)
    {
        CheckAddress(address, 1);

        return _memory[(int)address];
    }

    public void WriteByte(
        ulong address,
        byte value)
    {
        CheckAddress(address, 1);

        _memory[(int)address] = value;
    }

    public uint ReadUInt32(ulong address)
    {
        CheckAddress(address, 4);

        return (uint)(
            _memory[(int)address] |
            (_memory[(int)address + 1] << 8) |
            (_memory[(int)address + 2] << 16) |
            (_memory[(int)address + 3] << 24)
        );
    }

    public void WriteUInt32(
        ulong address,
        uint value)
    {
        CheckAddress(address, 4);

        _memory[(int)address] =
            (byte)(value & 0xFF);

        _memory[(int)address + 1] =
            (byte)((value >> 8) & 0xFF);

        _memory[(int)address + 2] =
            (byte)((value >> 16) & 0xFF);

        _memory[(int)address + 3] =
            (byte)((value >> 24) & 0xFF);
    }

    private void CheckAddress(
        ulong address,
        int size)
    {
        if (address + (ulong)size > (ulong)_memory.Length)
        {
            throw new IndexOutOfRangeException(
                $"Endereço de memória inválido: 0x{address:X}");
        }
    }
}

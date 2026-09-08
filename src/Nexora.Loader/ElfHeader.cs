namespace Nexora.Loader;

public sealed class ElfHeader
{
    public const ushort ElfClass64 = 2;
    public const ushort ElfDataLittleEndian = 1;
    public const ushort MachineAArch64 = 183;

    public byte[] Magic { get; } = new byte[4];

    public byte Class { get; set; }

    public byte Data { get; set; }

    public ushort Type { get; set; }

    public ushort Machine { get; set; }

    public uint Version { get; set; }

    public ulong EntryPoint { get; set; }

    public ulong ProgramHeaderOffset { get; set; }

    public ushort ProgramHeaderEntrySize { get; set; }

    public ushort ProgramHeaderCount { get; set; }

    public static bool IsElf(ReadOnlySpan<byte> data)
    {
        return data.Length >= 4 &&
               data[0] == 0x7F &&
               data[1] == (byte)'E' &&
               data[2] == (byte)'L' &&
               data[3] == (byte)'F';
    }

    public bool IsAArch64 =>
        Class == ElfClass64 &&
        Data == ElfDataLittleEndian &&
        Machine == MachineAArch64;
}

using System.Buffers.Binary;
using Nexora.Memory;

namespace Nexora.Loader;

public sealed class ElfLoader
{
    private readonly MemoryManager _memory;

    public ElfHeader? Header { get; private set; }

    public List<ElfProgramHeader> ProgramHeaders { get; } = new();

    public ulong EntryPoint =>
        Header?.EntryPoint ?? 0;

    public ElfLoader(MemoryManager memory)
    {
        _memory = memory;
    }

    public void Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(
                "ELF não encontrado.",
                path);

        byte[] data = File.ReadAllBytes(path);

        Load(data);
    }

    public void Load(ReadOnlySpan<byte> data)
    {
        if (!ElfHeader.IsElf(data))
            throw new InvalidDataException(
                "Arquivo não é um ELF.");

        if (data.Length < 64)
            throw new InvalidDataException(
                "Cabeçalho ELF64 incompleto.");

        var header = ParseHeader(data);

        if (!header.IsAArch64)
        {
            throw new NotSupportedException(
                $"ELF não é AArch64. Machine={header.Machine}");
        }

        Header = header;

        ProgramHeaders.Clear();

        ParseProgramHeaders(data, header);

        foreach (var program in ProgramHeaders)
        {
            if (program.IsLoadable)
                LoadSegment(data, program);
        }
    }

    private static ElfHeader ParseHeader(
        ReadOnlySpan<byte> data)
    {
        var header = new ElfHeader();

        data[..4].CopyTo(header.Magic);

        header.Class = data[4];
        header.Data = data[5];

        header.Type =
            BinaryPrimitives.ReadUInt16LittleEndian(
                data[16..18]);

        header.Machine =
            BinaryPrimitives.ReadUInt16LittleEndian(
                data[18..20]);

        header.Version =
            BinaryPrimitives.ReadUInt32LittleEndian(
                data[20..24]);

        header.EntryPoint =
            BinaryPrimitives.ReadUInt64LittleEndian(
                data[24..32]);

        header.ProgramHeaderOffset =
            BinaryPrimitives.ReadUInt64LittleEndian(
                data[32..40]);

        header.ProgramHeaderEntrySize =
            BinaryPrimitives.ReadUInt16LittleEndian(
                data[54..56]);

        header.ProgramHeaderCount =
            BinaryPrimitives.ReadUInt16LittleEndian(
                data[56..58]);

        return header;
    }

    private void ParseProgramHeaders(
        ReadOnlySpan<byte> data,
        ElfHeader header)
    {
        ulong offset = header.ProgramHeaderOffset;

        for (int i = 0; i < header.ProgramHeaderCount; i++)
        {
            ulong current =
                offset +
                ((ulong)i * header.ProgramHeaderEntrySize);

            if (current + 56 > (ulong)data.Length)
                throw new InvalidDataException(
                    "Program header ELF inválido.");

            var ph = new ElfProgramHeader();

            ph.Type =
                BinaryPrimitives.ReadUInt32LittleEndian(
                    data[(int)current..]);

            ph.Flags =
                BinaryPrimitives.ReadUInt32LittleEndian(
                    data[(int)current + 4..]);

            ph.Offset =
                BinaryPrimitives.ReadUInt64LittleEndian(
                    data[(int)current + 8..]);

            ph.VirtualAddress =
                BinaryPrimitives.ReadUInt64LittleEndian(
                    data[(int)current + 16..]);

            ph.PhysicalAddress =
                BinaryPrimitives.ReadUInt64LittleEndian(
                    data[(int)current + 24..]);

            ph.FileSize =
                BinaryPrimitives.ReadUInt64LittleEndian(
                    data[(int)current + 32..]);

            ph.MemorySize =
                BinaryPrimitives.ReadUInt64LittleEndian(
                    data[(int)current + 40..]);

            ph.Alignment =
                BinaryPrimitives.ReadUInt64LittleEndian(
                    data[(int)current + 48..]);

            ProgramHeaders.Add(ph);
        }
    }

    private void LoadSegment(
        ReadOnlySpan<byte> data,
        ElfProgramHeader segment)
    {
        if (segment.FileSize == 0)
            return;

        if (segment.Offset + segment.FileSize >
            (ulong)data.Length)
        {
            throw new InvalidDataException(
                "Segment ELF ultrapassa o arquivo.");
        }

        ulong address = segment.VirtualAddress;

        for (ulong i = 0; i < segment.FileSize; i++)
        {
            _memory.WriteByte(
                address + i,
                data[(int)(segment.Offset + i)]);
        }
    }
}

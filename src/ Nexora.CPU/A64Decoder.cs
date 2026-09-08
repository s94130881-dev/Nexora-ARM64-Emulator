namespace Nexora.CPU;

public static class A64Decoder
{
    public static A64Instruction Decode(uint raw)
    {
        return new A64Instruction(raw);
    }

    public static bool IsBranch(uint instruction)
    {
        return (instruction & 0x7C000000) == 0x14000000;
    }

    public static bool IsAddSubImmediate(uint instruction)
    {
        return (instruction & 0x1F000000) == 0x11000000;
    }

    public static bool IsLogicalImmediate(uint instruction)
    {
        return (instruction & 0x1F800000) == 0x12000000;
    }

    public static bool IsLoadStore(uint instruction)
    {
        return (instruction & 0x0C000000) == 0x04000000;
    }
}

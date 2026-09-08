namespace Nexora.CPU;

public sealed class CpuState
{
    public ulong[] X { get; } = new ulong[31];

    public ulong PC { get; set; }

    public ulong SP { get; set; }

    public uint PState { get; set; }

    public ulong X0
    {
        get => X[0];
        set => X[0] = value;
    }

    public ulong X1
    {
        get => X[1];
        set => X[1] = value;
    }

    public ulong X2
    {
        get => X[2];
        set => X[2] = value;
    }

    public ulong GetRegister(int index)
    {
        if ((uint)index >= 31)
            throw new ArgumentOutOfRangeException(nameof(index));

        return X[index];
    }

    public void SetRegister(int index, ulong value)
    {
        if ((uint)index >= 31)
            throw new ArgumentOutOfRangeException(nameof(index));

        X[index] = value;
    }

    public void Reset(ulong entryPoint)
    {
        Array.Clear(X);

        PC = entryPoint;
        SP = 0;
        PState = 0;
    }
}

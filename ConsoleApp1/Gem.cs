public class Gem
{
    public GemType Type;

    public Gem(GemType type = GemType.Empty)
    {
        Type = type;
    }

    public bool IsEmpty()
    {
        return Type == GemType.Empty;
    }

    public char ToChar()
    {
        if (Type == GemType.Red) return 'R';
        if (Type == GemType.Blue) return 'B';
        if (Type == GemType.Green) return 'G';
        if (Type == GemType.Yellow) return 'Y';
        if (Type == GemType.Purple) return 'P';
        return '.';
    }
}
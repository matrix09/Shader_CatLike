
public enum HexDir
{
    NE, E, SE, SW, W, NW,
}

//static extension method.
public static class HexDirectionExtensions
{
    public static HexDir Opposite(this HexDir dir)
    {
        return (int)dir < 3 ? (dir + 3) : (dir - 3);
    }
}


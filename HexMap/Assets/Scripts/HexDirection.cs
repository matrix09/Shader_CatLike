
public enum HexDir
{
    NE, E, SE, SW, W, NW,Size,
}

//static extension method.
public static class HexDirectionExtensions
{
    public static HexDir Opposite(this HexDir dir)
    {
        return (int)dir < 3 ? (dir + 3) : (dir - 3);
    }

    public static HexDir GetPreviousDir(HexDir dir)
    {
        return dir == HexDir.NE ? HexDir.NW : dir - 1;
    }

    public static HexDir GetNextDir(HexDir dir)
    {
        return dir == HexDir.NW ? HexDir.NE : dir + 1;
    }

}


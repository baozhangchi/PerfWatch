namespace PerfWatch;

public class GlobalCache
{
    private GlobalCache()
    {
    }

    public static GlobalCache Instance { get; } = new GlobalCache();
    public bool HasLogined { get; internal set; } = true;
}
using System.Collections.Generic;

public static class ResettableRegistry
{
    private static readonly List<IResettable> resettables = new();

    public static void Register(IResettable item)
    {
        if (!resettables.Contains(item)) resettables.Add(item);
    }

    public static void Unregister(IResettable item)
    {
        resettables.Remove(item);
    }

    public static void ResetAll()
    {
        foreach (var r in resettables)
            r.ResetToInitialState();
    }
}

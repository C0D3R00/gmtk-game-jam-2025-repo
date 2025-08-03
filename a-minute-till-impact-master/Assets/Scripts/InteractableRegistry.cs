using System.Collections.Generic;
using UnityEngine;

public class InteractableRegistry : MonoBehaviour
{
    public static Dictionary<string, IReplayable> replayables = new();

    public static void Register(string id, IReplayable obj) => replayables[id] = obj;
    public static IReplayable Find(string id) => replayables.TryGetValue(id, out var obj) ? obj : null;
}

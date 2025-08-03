using System;

[Serializable]
public struct InteractionFrame
{
    public int playerIndex; // 🧍 Index in LoopMaster.players
    public float timestamp;

    public string replayableId;
    public string action;

    public InteractionFrame(int index, float time, string id, string act)
    {
        playerIndex = index;
        timestamp = time;
        replayableId = id;
        action = act;
    }
}

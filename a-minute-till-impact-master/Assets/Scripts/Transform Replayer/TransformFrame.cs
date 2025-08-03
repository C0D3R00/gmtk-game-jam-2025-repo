using UnityEngine;

[System.Serializable]
public struct TransformFrame
{
    public int playerIndex; // 🧍 Index in LoopMaster.players
    public float timestamp;

    public Vector3 position;
    public Quaternion rotation;

    public TransformFrame(int index, float time, Vector3 pos, Quaternion rot)
    {
        playerIndex = index;
        timestamp = time;
        position = pos;
        rotation = rot;
    }
}

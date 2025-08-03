using System;
using UnityEngine;

[Serializable]
public struct LoopDialogue
{
    public int loopIndex;
    [TextArea(2, 5)]
    public string[] lines;
}

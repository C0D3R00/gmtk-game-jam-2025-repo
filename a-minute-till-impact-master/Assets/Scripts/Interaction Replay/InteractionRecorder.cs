using System.Collections.Generic;
using UnityEngine;

public class InteractionRecorder : MonoBehaviour
{
    public List<InteractionFrame> interactionFrames = new();

    private float startTime;
    private bool isRecording = false;

    public void StartRecording()
    {
        interactionFrames.Clear();
        startTime = Time.time;
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void RecordInteraction(IReplayable target, string action)
    {
        if (!isRecording) return;

        float timestamp = Time.time - startTime;
        interactionFrames.Add(new InteractionFrame
        {
            timestamp = timestamp,
            replayableId = target.Id,
            action = action
        });

        Debug.Log($"[InteractionRecorder] {target.Id} → {action} @ {timestamp:0.00}s");
    }
}
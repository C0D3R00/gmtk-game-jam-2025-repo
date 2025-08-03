using System.Collections.Generic;
using UnityEngine;

public class InteractionReplayer : MonoBehaviour
{
    public List<InteractionFrame> interactionFrames;
    private int currentIndex = 0;
    private float startTime;

    private bool isReplaying = false;

    public void StartReplay(List<InteractionFrame> frames)
    {
        interactionFrames = frames;
        currentIndex = 0;
        startTime = Time.time;
        isReplaying = true;
    }

    public void StopReplay()
    {
        isReplaying = false;
    }

    void Update()
    {
        if (!isReplaying || interactionFrames == null || currentIndex >= interactionFrames.Count)
            return;

        float elapsed = Time.time - startTime;

        while (currentIndex < interactionFrames.Count &&
               interactionFrames[currentIndex].timestamp <= elapsed)
        {
            var frame = interactionFrames[currentIndex];

            var replayable = InteractableRegistry.Find(frame.replayableId);
            if (replayable != null)
            {
                replayable.ReplayAction(frame.action);
                Debug.Log($"[InteractionReplayer] Replayed: {frame.replayableId} → {frame.action} @ {frame.timestamp:0.00}s");
            }
            else
            {
                Debug.LogWarning($"[InteractionReplayer] Could not find replayable ID: {frame.replayableId}");
            }

            currentIndex++;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TransformReplayer : MonoBehaviour
{
    public List<TransformFrame> frames = new();
    private float startTime;
    private int currentIndex = 0;

    public void StartReplay(List<TransformFrame> recordedFrames)
    {
        frames = recordedFrames;
        currentIndex = 0;
        startTime = Time.time;
    }

    void LateUpdate() // more stable than Update()
    {
        if (frames == null || frames.Count < 2 || currentIndex >= frames.Count - 1)
            return;

        float elapsed = Time.time - startTime;

        while (currentIndex < frames.Count - 1 && elapsed > frames[currentIndex + 1].timestamp)
        {
            currentIndex++;
        }

        var from = frames[currentIndex];
        var to = frames[currentIndex + 1];
        float t = Mathf.InverseLerp(from.timestamp, to.timestamp, elapsed);

        transform.position = Vector3.Lerp(from.position, to.position, t);
        transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);
    }
}

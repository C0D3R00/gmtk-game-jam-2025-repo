using System.Collections.Generic;
using UnityEngine;

public class LoopReplayerSystem : MonoBehaviour
{
    public static LoopReplayerSystem Instance { get; private set; }

    private class ReplayState
    {
        public GameObject player;
        public List<TransformFrame> transformFrames;
        public List<InteractionFrame> interactionFrames;
        public float startTime;
        public int transformIndex = 0;
        public int interactionIndex = 0;

        public CharacterController charController;
        public Rigidbody rigidbody;
    }

    private readonly List<ReplayState> replays = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        float now = Time.time;

        foreach (var replay in replays)
        {
            float t = now - replay.startTime;

            var frames = replay.transformFrames;
            if (frames.Count >= 2)
            {
                // Advance only if the time exceeds next frame
                if (replay.transformIndex < frames.Count - 2 &&
                    t > frames[replay.transformIndex + 1].timestamp)
                {
                    replay.transformIndex++;
                }

                int index = replay.transformIndex;
                var before = frames[index];
                var after = frames[index + 1];

                float delta = after.timestamp - before.timestamp;
                float lerpT = delta > 0f
                    ? Mathf.Clamp01((t - before.timestamp) / delta)
                    : 1f; // Avoid divide by zero

                replay.player.transform.SetPositionAndRotation(Vector3.Lerp(before.position, after.position, lerpT), Quaternion.Slerp(before.rotation, after.rotation, lerpT));

                // Debugging (optional)
                // Debug.Log($"Replay [{index}] t={t:F2} lerpT={lerpT:F2} time=({before.timestamp:F2}, {after.timestamp:F2})");
                // Debug.Log($"{replay.player.name} - {transform.position}");
                // Debug.Log($"[{replay.player.name}] t={t:F2}, currentFrame={replay.transformIndex}, nextTime={frames[replay.transformIndex + 1].timestamp:F2}");
                // Debug.Log($"{replay.player.name} - {replay.transformFrames[replay.transformIndex].position}");
            }

            // Trigger interactions
            while (replay.interactionIndex < replay.interactionFrames.Count &&
                   replay.interactionFrames[replay.interactionIndex].timestamp <= t)
            {
                var frame = replay.interactionFrames[replay.interactionIndex];
                var target = InteractableRegistry.Find(frame.replayableId);
                target?.ReplayAction(frame.action);
                replay.interactionIndex++;
            }
        }
    }

    public void StartReplay(GameObject player, List<TransformFrame> transformFrames, List<InteractionFrame> interactionFrames)
    {
        var cc = player.GetComponent<CharacterController>();
        var rb = player.GetComponent<Rigidbody>();

        if (cc) cc.enabled = false;
        if (rb) rb.isKinematic = true;

        replays.Add(new ReplayState
        {
            player = player,
            transformFrames = transformFrames,
            interactionFrames = interactionFrames,
            startTime = Time.time,
            charController = cc,
            rigidbody = rb
        });
    }

    public void ClearReplays()
    {
        foreach (var r in replays)
        {
            if (r.charController) r.charController.enabled = true;
            if (r.rigidbody) r.rigidbody.isKinematic = false;
        }

        replays.Clear();
    }
}

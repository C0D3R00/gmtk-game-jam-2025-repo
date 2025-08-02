using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplayer : MonoBehaviour
{
    public CharacterController controller;
    public FirstPersonController fpc;

    private List<PlayerActionFrame> frames;
    private List<PlayerInteractionEvent> events;
    private float currentTime;
    private int frameIndex;
    private int eventIndex;

    private void Update()
    {
        if (frames == null) return;

        currentTime += Time.deltaTime;
        if (frameIndex < frames.Count && frames[frameIndex].time <= currentTime)
        {
            var frame = frames[frameIndex];
            ApplyMovement(frame);
            frameIndex++;
        }

        while (eventIndex < events.Count && events[eventIndex].time <= currentTime)
        {
            TriggerInteraction(events[eventIndex]);
            eventIndex++;
        }
    }

    private void ApplyMovement(PlayerActionFrame frame)
    {
        Vector3 move = new Vector3(frame.moveInput.x, 0, frame.moveInput.y);
        move = transform.TransformDirection(move);
        controller.SimpleMove(move * 5f); // replace with your FPC speed
    }

    private void TriggerInteraction(PlayerInteractionEvent interaction)
    {
        GameObject target = GameObject.Find(interaction.targetId);
        if (target && target.TryGetComponent<IReplayable>(out var replayable))
        {
            replayable.ReplayAction(interaction.action);
        }
    }

    public void BeginPlayback(List<PlayerActionFrame> f, List<PlayerInteractionEvent> e)
    {
        Debug.Log($"begin playback frames.count: {f.Count}");
        frames = f;
        events = e;
        currentTime = 0f;
        frameIndex = 0;
        eventIndex = 0;

        fpc.DisableInput();
    }
}

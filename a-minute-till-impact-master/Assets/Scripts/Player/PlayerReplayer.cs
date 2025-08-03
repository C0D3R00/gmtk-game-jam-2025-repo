using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
        if (frames == null || frames.Count == 0) return;

        currentTime += Time.deltaTime;

        if (frameIndex < frames.Count && frames[frameIndex].time <= currentTime)
        {

            Debug.Log($"🔁 {this.name} Replay Frame #{frameIndex} @ {currentTime:F2}s | Move: {frames[frameIndex].moveInput} Look: {frames[frameIndex].lookInput}");
            ApplyMovement(frames[frameIndex]);
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
        Vector3 localInput = new Vector3(frame.moveInput.x, 0, frame.moveInput.y);
        Vector3 worldInput = transform.TransformDirection(localInput);

        float speed = fpc.MoveSpeed;
        Vector3 deltaMove = worldInput * speed * Time.deltaTime;

        controller.Move(deltaMove);
    }

    private void TriggerInteraction(PlayerInteractionEvent interaction)
    {
        var target = GameObject.Find(interaction.targetId);
        if (target && target.TryGetComponent<IReplayable>(out var replayable))
        {
            replayable.ReplayAction(interaction.action);
        }
    }

    public void BeginPlayback(List<PlayerActionFrame> f, List<PlayerInteractionEvent> e)
    {
        frames = f;
        events = e ?? new();

        currentTime = 0f;
        frameIndex = 0;
        eventIndex = 0;

        if (frames[0].worldPosition.HasValue)
        {
            controller.enabled = false;
            transform.position = frames[0].worldPosition.Value;
            transform.rotation = frames[0].worldRotation ?? Quaternion.identity;
            controller.enabled = true;
        }

        while (frameIndex < frames.Count &&
               frames[frameIndex].moveInput == Vector2.zero &&
               frames[frameIndex].lookInput == Vector2.zero)
        {
            Debug.Log($"⚠️ Skipping idle frame {frameIndex}");
            frameIndex++;
        }

        if (fpc)
        {
            fpc.enabled = false;
            fpc.DisableInput();
        }

        Debug.Log($"▶️ Replay started at frame #{frameIndex}, total: {frames.Count}");
    }
}

using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplayer : MonoBehaviour
{
    private FirstPersonController fpc;
    private StarterAssetsInputs _input;
    private CharacterController controller;

    private List<PlayerActionFrame> frames;
    private List<PlayerInteractionEvent> events;
    private float currentTime;
    private int frameIndex;
    private int eventIndex;

    private bool isReplaying = false;

    private void Awake()
    {
        fpc = GetComponent<FirstPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!isReplaying || frames == null || frameIndex >= frames.Count)
            return;

        currentTime += Time.deltaTime;

        // Process movement frames
        if (frameIndex < frames.Count && frames[frameIndex].time <= currentTime)
        {
            var frame = frames[frameIndex];
            Debug.Log($"🔁{this.name} Replay Frame #{frameIndex} @ {currentTime:F2}s | Move: {frame.moveInput}, Look: {frame.lookInput}");

            // Feed recorded input into StarterAssetsInputs
            _input.move = frame.moveInput;
            _input.look = frame.lookInput;
            _input.jump = frame.jump;
            //_input.interact = frame.interact;
            //_input.crouch = frame.crouch;
            _input.sprint = frame.sprint;

            frameIndex++;
        }

        // Process interaction events
        while (eventIndex < events.Count && events[eventIndex].time <= currentTime)
        {
            var interaction = events[eventIndex];
            Debug.Log($"🎬 Triggering interaction '{interaction.action}' on '{interaction.targetId}' at {interaction.time:F2}s");

            TriggerInteraction(interaction);
            eventIndex++;
        }
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
        frames = f;
        events = e ?? new List<PlayerInteractionEvent>();
        currentTime = 0f;
        frameIndex = 0;
        eventIndex = 0;
        isReplaying = true;

        // Reset inputs
        if (_input != null)
        {
            _input.move = Vector2.zero;
            _input.look = Vector2.zero;
            _input.jump = false;
            //_input.interact = false;
            //_input.crouch = false;
            _input.sprint = false;
        }

        fpc.EnableInput();
        Debug.Log($"▶️ Replaying {frames.Count} frames from time 0");
    }

    public void StopReplay()
    {
        isReplaying = false;

        // Reset inputs after replay
        if (_input != null)
        {
            _input.move = Vector2.zero;
            _input.look = Vector2.zero;
            _input.jump = false;
            //_input.interact = false;
            //_input.crouch = false;
            _input.sprint = false;
        }

        Debug.Log("⏹️ Replay finished.");
    }
}

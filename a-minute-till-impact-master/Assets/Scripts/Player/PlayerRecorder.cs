using UnityEngine;
using System.Collections.Generic;

public class PlayerRecorder : MonoBehaviour
{
    public List<PlayerActionFrame> recordedFrames = new();
    public List<PlayerInteractionEvent> interactionEvents = new();

    private bool _isRecording = false;
    public bool IsRecording => _isRecording;

    private float timeElapsed = 0f;
    private float recordInterval = 0.05f;
    private float timer = 0f;

    public Vector2 currentMoveInput;
    public Vector2 currentLookInput;
    public bool currentJump;
    public bool currentInteract;
    public bool currentSprint;
    public bool currentCrouch;

    public void SetInput(Vector2 move, Vector2 look, bool jump, bool interact, bool sprint, bool crouch)
    {
        currentMoveInput = move;
        currentLookInput = look;
        currentJump = jump;
        currentInteract = interact;
        currentSprint = sprint;
        currentCrouch = crouch;
    }

    private void Update()
    {
        if (!_isRecording) return;

        timeElapsed += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= recordInterval)
        {
            bool isNonZeroInput =
                currentMoveInput != Vector2.zero ||
                currentLookInput != Vector2.zero ||
                currentJump || currentSprint || currentCrouch || currentInteract;

            // Always record the first frame, or only record if input is non-zero
            if (recordedFrames.Count == 0 || isNonZeroInput)
            {
                recordedFrames.Add(new PlayerActionFrame
                {
                    time = timeElapsed,
                    moveInput = currentMoveInput,
                    lookInput = currentLookInput,
                    jump = currentJump,
                    interact = currentInteract,
                    crouch = currentCrouch,
                    sprint = currentSprint
                });

                Debug.Log($"🟢{this.name} Recorded @ {timeElapsed:F2}s | Move: {currentMoveInput}, Look: {currentLookInput}");
            }
            else
            {
                Debug.Log($"⏭️{this.name} Skipped frame @ {timeElapsed:F2}s (no input)");
            }

            timer = 0f;
        }
    }

    public void RecordInteraction(string id, string action)
    {
        interactionEvents.Add(new PlayerInteractionEvent
        {
            time = timeElapsed,
            targetId = id,
            action = action
        });
    }

    public void StartRecording()
    {
        ClearRecording(); // Always clear before new loop
        _isRecording = true;
    }

    public void StopRecording()
    {
        _isRecording = false;
    }

    public void FinishRecording()
    {
        StopRecording(); // In case we want custom logic later
    }

    public void ClearRecording()
    {
        recordedFrames.Clear();
        interactionEvents.Clear();
        timeElapsed = 0f;
        timer = 0f;
    }
}

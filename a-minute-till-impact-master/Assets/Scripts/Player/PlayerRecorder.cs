using UnityEngine;
using System.Collections.Generic;

public class PlayerRecorder : MonoBehaviour
{
    public List<PlayerActionFrame> recordedFrames = new();
    public List<PlayerInteractionEvent> interactionEvents = new();

    public bool isRecording = false;

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
        if (!isRecording) return;

        timeElapsed += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= recordInterval)
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

            Debug.Log($"timeElapsed: {timeElapsed} currentMoveInput: {currentMoveInput} currentLookInput: {currentLookInput}");
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

    public void ClearRecording()
    {
        recordedFrames.Clear();
        interactionEvents.Clear();
        timeElapsed = 0f;
        timer = 0f;
    }

    public void FinishRecording()
    {
        isRecording = false;
    }
}

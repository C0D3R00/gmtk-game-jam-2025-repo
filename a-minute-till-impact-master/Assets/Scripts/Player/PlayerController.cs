using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private FirstPersonController fpc;
    private PlayerRecorder recorder;
    private PlayerReplayer replayer;
    private PlayerInput playerInput;
    private StarterAssetsInputs _input;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private void Awake()
    {
        fpc = GetComponent<FirstPersonController>();
        recorder = GetComponent<PlayerRecorder>();
        replayer = GetComponent<PlayerReplayer>();
        playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (recorder != null && recorder.IsRecording && _input != null)
        {
            recorder.SetInput(
                _input.move,
                _input.look,
                _input.jump,
                false, // Interact
                _input.sprint,
                false  // Crouch
            );
        }
    }

    public void EnableControl()
    {
        if (playerInput) playerInput.enabled = true;

        fpc.enabled = true;        // 🟢 Re-enable the whole script
        fpc.EnableInput();         // 🟢 Re-enable input logic
        recorder.StartRecording(); // 🔴 Start recording after one frame (optional: coroutine)
    }


    //private System.Collections.IEnumerator DelayedStartRecording()
    //{
    //    yield return null;
    //    recorder.StartRecording(); // properly encapsulated
    //}

    public void DisableControl()
    {
        fpc.DisableInput();
        recorder.StopRecording();

        if (playerInput) playerInput.enabled = false;
    }

    public void DisableAll()
    {
        fpc.DisableInput();
        recorder.StopRecording();

        if (playerInput) playerInput.enabled = false;

        if (_input != null)
        {
            _input.move = Vector2.zero;
            _input.look = Vector2.zero;
            _input.jump = false;
            _input.sprint = false;
        }
    }

    public void PrepareForNewLoop()
    {
        recorder.ClearRecording();

        if (_input != null)
        {
            _input.move = Vector2.zero;
            _input.look = Vector2.zero;
            _input.jump = false;
            _input.sprint = false;
        }

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void FinishRecording()
    {
        recorder.FinishRecording(); // internally calls StopRecording
    }

    public void StartReplay(List<PlayerActionFrame> frames, List<PlayerInteractionEvent> events)
    {
        recorder.StopRecording();                 // 🛑 No recording during replay
        fpc.DisableInput();                       // ⛔ Stop reading input
        fpc.enabled = false;                      // ❌ Fully stop character logic (including gravity)
        if (playerInput) playerInput.enabled = false;

        replayer.BeginPlayback(frames, events);   // 🔁 Let replay take over
    }



    public void SetCheckpoint()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    public void ResetToCheckpoint()
    {
        var characterController = GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        transform.position = startingPosition;
        transform.rotation = startingRotation;

        if (characterController != null) characterController.enabled = true;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

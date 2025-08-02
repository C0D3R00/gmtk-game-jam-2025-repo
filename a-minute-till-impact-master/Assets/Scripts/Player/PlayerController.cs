using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private FirstPersonController fpc;
    private PlayerRecorder recorder;
    private PlayerReplayer replayer;
    private PlayerInput playerInput;
    private StarterAssetsInputs _input;

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
        if (recorder != null && recorder.isRecording && _input != null)
        {
            recorder.SetInput(
                _input.move, 
                _input.look,
                _input.jump,
                false,
                _input.sprint,
                false);
        }
    }
    public void EnableControl()
    {
        if (playerInput) playerInput.enabled = true;
        fpc.EnableInput();
        recorder.isRecording = true;
    }

    public void DisableControl()
    {
        fpc.DisableInput();
        recorder.isRecording = false;
        if (playerInput) playerInput.enabled = false;
    }

    public void DisableAll()
    {
        fpc.DisableInput();
        recorder.isRecording = false;
        if (playerInput) playerInput.enabled = false;
    }

    public void FinishRecording()
    {
        recorder.FinishRecording();
    }

    public void StartReplay(List<PlayerActionFrame> frames, List<PlayerInteractionEvent> events)
    {
        replayer.BeginPlayback(frames, events);
    }
}

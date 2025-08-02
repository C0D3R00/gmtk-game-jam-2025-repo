using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private FirstPersonController fpc;
    private PlayerRecorder recorder;
    private PlayerReplayer replayer;

    private void Awake()
    {
        fpc = GetComponent<FirstPersonController>();
        recorder = GetComponent<PlayerRecorder>();
        replayer = GetComponent<PlayerReplayer>();
    }

    public void EnableControl()
    {
        fpc.EnableInput();
        recorder.isRecording = true;
    }

    public void DisableControl()
    {
        fpc.DisableInput();
        recorder.isRecording = false;
    }

    public void DisableAll()
    {
        fpc.DisableInput();
        recorder.isRecording = false;
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

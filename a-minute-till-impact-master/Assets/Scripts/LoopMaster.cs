using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LoopMaster : MonoBehaviour
{
    public static LoopMaster Instance;

    [Header("Loop Settings")]
    [SerializeField] private float loopDuration = 60f;
    [SerializeField] private List<PlayerController> players; // Player1, Player2, Player3...
    [SerializeField] private string cameraAnchorName = "CameraAnchor";

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    private List<List<PlayerActionFrame>> frameHistory = new();
    private List<List<PlayerInteractionEvent>> interactionHistory = new();

    private int currentLoop = 0;
    private float loopTimer;
    private bool loopRunning = false;
    private bool puzzleSolved = false;

    private Camera mainCam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        mainCam = Camera.main;
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        StartLoop();
    }

    private void Update()
    {
        if (!loopRunning || puzzleSolved) return;

        loopTimer -= Time.deltaTime;
        if (loopTimer <= 0)
        {
            EndLoop();
        }
    }

    public void StartLoop()
    {
        loopTimer = loopDuration;
        loopRunning = true;

        Debug.Log($"🔁 Starting Loop #{currentLoop + 1}");

        int activeIndex = currentLoop % players.Count;

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];

            if (i == activeIndex)
            {
                Debug.Log($"player.name: {player.name}");
                player.EnableControl();
                SetActiveAudioListener(player);
                MoveCameraToPlayer(player);
            }
            else if (i < frameHistory.Count)
            {
                var frames = frameHistory[i];
                var events = interactionHistory[i];
                player.StartReplay(frames, events);
            }
            else
            {
                player.DisableAll();
            }
        }
    }

    private void MoveCameraToPlayer(PlayerController player)
    {
        var anchor = player.transform.Find(cameraAnchorName);
        if (anchor == null)
        {
            Debug.LogWarning($"No CameraAnchor found on {player.name}");
            return;
        }

        if (virtualCam != null)
        {
            virtualCam.Follow = anchor;
            virtualCam.LookAt = anchor;
        }
        else if (mainCam != null)
        {
            mainCam.transform.SetParent(anchor);
            mainCam.transform.localPosition = Vector3.zero;
            mainCam.transform.localRotation = Quaternion.identity;
        }
    }
    private void SetActiveAudioListener(PlayerController player)
    {
        foreach (var pc in players)
        {
            var listener = pc.GetComponentInChildren<AudioListener>();
            if (listener) listener.enabled = (pc == player);
        }
    }

    public void EndLoop()
    {
        loopRunning = false;

        int activeIndex = currentLoop % players.Count;
        var activePlayer = players[activeIndex];
        var recorder = activePlayer.GetComponent<PlayerRecorder>();

        activePlayer.DisableControl();
        recorder.FinishRecording();

        frameHistory.Add(new List<PlayerActionFrame>(recorder.recordedFrames));
        interactionHistory.Add(new List<PlayerInteractionEvent>(recorder.interactionEvents));
        recorder.ClearRecording();

        currentLoop++;

        if (puzzleSolved)
        {
            Debug.Log("✅ Puzzle solved! Ending loop system.");
            return;
        }

        StartCoroutine(RestartLoop());
    }

    private IEnumerator RestartLoop()
    {
        Debug.Log("💥 Boom! Restarting loop in 2s...");
        yield return new WaitForSeconds(2f);

        StartLoop();
    }

    public void MarkPuzzleSolved()
    {
        puzzleSolved = true;
        Debug.Log("🎉 Puzzle solved — loop will stop after current run.");
    }
}

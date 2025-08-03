using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class LoopMaster : MonoBehaviour
{
    public static LoopMaster Instance;

    [Header("Loop Settings")]
    [SerializeField] private float loopDuration = 60f;
    [SerializeField] private List<PlayerController> players;
    [SerializeField] private string cameraAnchorName = "CameraAnchor";
    [SerializeField] private TMP_Text countdownTmp;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCam;

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

        foreach (var player in players)
        {
            player.SetCheckpoint();
        }

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
        int secondsLeft = Mathf.FloorToInt(loopTimer);
        countdownTmp.text = $"00:<color=red>{secondsLeft}</color>";
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

        LoopReplayerSystem.Instance.ClearReplays();
        ResettableRegistry.ResetAll();
        DialogueManager.Instance.PlayNextDialogue();

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            player.ResetToCheckpoint();

            if (i == activeIndex)
            {
                player.EnableControl();
                LoopRecorderSystem.Instance.StartRecording(i, player.gameObject);

                SetActiveAudioListener(player);
                MoveCameraToPlayer(player);
            }
            else
            {
                player.DisableAll();

                var tf = LoopRecorderSystem.Instance.GetTransformFrames(i);
                var ia = LoopRecorderSystem.Instance.GetInteractionFrames(i);
                var af = LoopRecorderSystem.Instance.GetAnimationFrames(i); // ✅ get animation frames

                if (tf.Count > 0 || ia.Count > 0 || af.Count > 0)
                {
                    LoopReplayerSystem.Instance.StartReplay(player.gameObject, tf, ia, af); // ✅ pass animation frames
                }
            }
        }
    }

    public void EndLoop()
    {
        loopRunning = false;

        int activeIndex = currentLoop % players.Count;
        var activePlayer = players[activeIndex];

        activePlayer.DisableControl();
        LoopRecorderSystem.Instance.StopRecording();
        activePlayer.PrepareForNewLoop();

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

    private void MoveCameraToPlayer(PlayerController player)
    {
        var anchor = player.transform.Find(cameraAnchorName);
        if (anchor == null)
        {
            Debug.LogWarning($"⚠️ No CameraAnchor found on {player.name}");
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

    public void MarkPuzzleSolved()
    {
        puzzleSolved = true;
        Debug.Log("🎉 Puzzle solved — loop will stop after current run.");
    }
}

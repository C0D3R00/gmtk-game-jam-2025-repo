using System.Collections.Generic;
using UnityEngine;

public class LoopRecorderSystem : MonoBehaviour
{
    public static LoopRecorderSystem Instance { get; private set; }

    private float startTime;
    private bool isRecording = false;

    private int activePlayerIndex;
    private GameObject activePlayer;

    private readonly Dictionary<int, List<TransformFrame>> transformHistory = new();
    private readonly Dictionary<int, List<InteractionFrame>> interactionHistory = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void FixedUpdate()
    {
        if (!isRecording || activePlayer == null) return;

        float timestamp = Time.time - startTime;
        if (!transformHistory.ContainsKey(activePlayerIndex))
            transformHistory[activePlayerIndex] = new();

        transformHistory[activePlayerIndex].Add(new TransformFrame(
            activePlayerIndex,
            timestamp,
            activePlayer.transform.position,
            activePlayer.transform.rotation
        ));
    }

    public void StartRecording(int playerIndex, GameObject player)
    {
        activePlayerIndex = playerIndex;
        activePlayer = player;
        startTime = Time.time;
        isRecording = true;

        if (!transformHistory.ContainsKey(playerIndex))
            transformHistory[playerIndex] = new();

        if (!interactionHistory.ContainsKey(playerIndex))
            interactionHistory[playerIndex] = new();
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void ClearAll()
    {
        transformHistory.Clear();
        interactionHistory.Clear();
    }

    public void RecordInteraction(string replayableId, string action)
    {
        if (!isRecording || string.IsNullOrEmpty(replayableId)) return;

        float timestamp = Time.time - startTime;

        if (!interactionHistory.ContainsKey(activePlayerIndex))
            interactionHistory[activePlayerIndex] = new();

        interactionHistory[activePlayerIndex].Add(new InteractionFrame(
            activePlayerIndex,
            timestamp,
            replayableId,
            action
        ));
    }

    public List<TransformFrame> GetTransformFrames(int playerIndex) =>
        transformHistory.TryGetValue(playerIndex, out var frames) ? new List<TransformFrame>(frames) : new();

    public List<InteractionFrame> GetInteractionFrames(int playerIndex) =>
        interactionHistory.TryGetValue(playerIndex, out var frames) ? new List<InteractionFrame>(frames) : new();
}

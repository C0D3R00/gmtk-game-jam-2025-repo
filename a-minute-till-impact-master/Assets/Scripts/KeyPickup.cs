using UnityEngine;

public class KeyPickup : MonoBehaviour, IReplayable
{
    [SerializeField] private string keyId = "Key_001";
    public string Id => keyId;

    private void Awake()
    {
        InteractableRegistry.Register(keyId, this);
    }

    public void ReplayAction(string action)
    {
        if (action == "pickup")
        {
            // Simulate pickup (e.g., hide from ghost player)
            gameObject.SetActive(false);
        }
    }

    public void OnInteract(GameObject player)
    {
        // Normal interaction logic...
        var recorder = player.GetComponent<PlayerRecorder>();
        if (recorder != null && recorder.isRecording)
        {
            recorder.RecordInteraction(Id, "pickup");
        }

        gameObject.SetActive(false);
    }
}

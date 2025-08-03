using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndZoneTrigger : MonoBehaviour
{
    [Header("Player References")]
    public GameObject[] players; // Assign your 3 players here in the Inspector

    [Header("Event")]
    public UnityEvent onAllPlayersInside;

    private HashSet<GameObject> playersInZone = new();

    private void OnTriggerEnter(Collider other)
    {
        foreach (var player in players)
        {
            if (other.gameObject == player && !playersInZone.Contains(player))
            {
                playersInZone.Add(player);
                CheckAllPlayersInside();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var player in players)
        {
            if (other.gameObject == player && playersInZone.Contains(player))
            {
                playersInZone.Remove(player);
            }
        }
    }

    private void CheckAllPlayersInside()
    {
        if (playersInZone.Count == players.Length)
        {
            Debug.Log("✅ All players are inside! Ending game...");
            SceneManager.LoadScene(0);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private HashSet<string> keyInventory = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void AddKey(string keyId)
    {
        keyInventory.Add(keyId);
    }

    public bool HasKey(string keyId)
    {
        return keyInventory.Contains(keyId);
    }
}

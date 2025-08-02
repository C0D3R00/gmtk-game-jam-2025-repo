using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask fullBlockingLayer; // All colliders, including walls, doors, etc.

    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Camera cam;
    private IInteractable currentTarget;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, fullBlockingLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                currentTarget = interactable;

                SwitchPromptUI.Instance.ShowPrompt(true, currentTarget.GetPrompt());

                if (Input.GetKeyDown(interactKey))
                    currentTarget.Interact();

                return;
            }
        }

        currentTarget = null;
        SwitchPromptUI.Instance.ShowPrompt(false);
    }
}

using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Camera cam;
    private ISwitchTarget currentTarget;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            currentTarget = hit.collider.GetComponent<ISwitchTarget>();
            if (currentTarget != null)
            {
                SwitchPromptUI.Instance.ShowPrompt(true); // show UI

                if (Input.GetKeyDown(interactKey))
                    currentTarget.Activate();
            }
            else
            {
                SwitchPromptUI.Instance.ShowPrompt(false);
            }
        }
        else
        {
            currentTarget = null;
            SwitchPromptUI.Instance.ShowPrompt(false);
        }
    }
}

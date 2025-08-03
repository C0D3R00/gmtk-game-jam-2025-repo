using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private FirstPersonController fpc;
    private PlayerInput playerInput;
    private StarterAssetsInputs _input;
    private CharacterController characterController;
    private PlayerInteractor interactor;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private void Awake()
    {
        fpc = GetComponent<FirstPersonController>();
        playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
        characterController = GetComponent<CharacterController>();
        interactor = GetComponent<PlayerInteractor>();
    }

    public void EnableControl()
    {
        if (characterController) characterController.enabled = true;
        if (playerInput) playerInput.enabled = true;
        if (fpc)
        {
            fpc.enabled = true;
            fpc.EnableInput();
        }
        if (interactor) interactor.enabled = true;
    }

    public void DisableControl()
    {
        if (fpc) fpc.DisableInput();
        if (playerInput) playerInput.enabled = false;
    }

    public void DisableAll()
    {
        if (characterController) characterController.enabled = false;
        if (fpc)
        {
            fpc.DisableInput();
            fpc.enabled = false;
        }
        if (playerInput) playerInput.enabled = false;
        if (interactor) interactor.enabled = false;

        if (_input != null)
        {
            _input.move = Vector2.zero;
            _input.look = Vector2.zero;
            _input.jump = false;
            _input.sprint = false;
        }
    }

    public void PrepareForNewLoop()
    {
        if (_input != null)
        {
            _input.move = Vector2.zero;
            _input.look = Vector2.zero;
            _input.jump = false;
            _input.sprint = false;
        }

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void SetCheckpoint()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    public void ResetToCheckpoint()
    {
        if (characterController) characterController.enabled = false;

        transform.position = startingPosition;
        transform.rotation = startingRotation;

        if (characterController) characterController.enabled = true;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

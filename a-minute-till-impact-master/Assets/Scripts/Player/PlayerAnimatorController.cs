using StarterAssets;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private StarterAssetsInputs input;

    private PlayerAnimState lastState = PlayerAnimState.Idle;
    private enum PlayerAnimState
    {
        Idle = 0,
        Walk = 1,
        Run = 2
    }

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<StarterAssetsInputs>();
    }


    private void FixedUpdate()
    {
        if (animator == null || input == null) return;

        PlayerAnimState state = GetMovementState();
        if (state != lastState)
        {
            animator.SetInteger("animState", (int)state);
            lastState = state;

            LoopRecorderSystem.Instance.RecordAnimation((int)state);
        }
    }


    private PlayerAnimState GetMovementState()
    {
        if (input.move == Vector2.zero) return PlayerAnimState.Idle;
        if (input.sprint) return PlayerAnimState.Run;
        return PlayerAnimState.Walk;
    }
}

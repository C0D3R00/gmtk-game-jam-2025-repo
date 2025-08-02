using UnityEngine;

public abstract class DoorBase : MonoBehaviour
{
    public enum DoorMovementType { Slide, Rotate }

    [Header("Door Movement")]
    [SerializeField] protected DoorMovementType movementType = DoorMovementType.Slide;
    [SerializeField] protected Transform openPoint;
    [SerializeField] protected Transform closedPoint;
    [SerializeField] protected float speed = 3f;

    protected bool isOpen = false;

    protected virtual void Update()
    {
        if (openPoint == null || closedPoint == null) return;

        if (movementType == DoorMovementType.Slide)
        {
            Vector3 target = isOpen ? openPoint.position : closedPoint.position;
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        }
        else if (movementType == DoorMovementType.Rotate)
        {
            Quaternion target = isOpen ? openPoint.rotation : closedPoint.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, target, speed * Time.deltaTime);
        }
    }

    public virtual void Open() => isOpen = true;
    public virtual void Close() => isOpen = false;
    public virtual void Toggle() => isOpen = !isOpen;
}

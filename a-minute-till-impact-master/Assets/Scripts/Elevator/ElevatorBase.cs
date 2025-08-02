using UnityEngine;

public abstract class ElevatorBase : MonoBehaviour
{
    [Header("Elevator Settings")]
    [SerializeField] protected Transform upperPoint;
    [SerializeField] protected Transform lowerPoint;
    [SerializeField] protected float speed = 2f;

    protected bool isAtTop = false;

    protected virtual void Update()
    {
        if (upperPoint == null || lowerPoint == null) return;

        Vector3 target = isAtTop ? upperPoint.position : lowerPoint.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public virtual void GoUp() => isAtTop = true;
    public virtual void GoDown() => isAtTop = false;
    public virtual void Toggle() => isAtTop = !isAtTop;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

}

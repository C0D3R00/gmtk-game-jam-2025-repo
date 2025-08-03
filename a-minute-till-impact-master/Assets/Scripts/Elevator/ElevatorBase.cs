using Mono.Cecil.Cil;
using UnityEngine;

public abstract class ElevatorBase : MonoBehaviour, IResettable
{
    [Header("Elevator Settings")]
    [SerializeField] protected Transform upperPoint;
    [SerializeField] protected Transform lowerPoint;
    [SerializeField] protected float speed = 2f;
    [SerializeField] private bool startAtTop = false;

    protected bool isAtTop = false;
    private Vector3 initialPosition;

    protected virtual void Awake()
    {
        initialPosition = transform.position;
        isAtTop = startAtTop;
    }
    protected virtual void OnEnable()
    {
        ResettableRegistry.Register(this);
    }

    protected virtual void OnDisable()
    {
        ResettableRegistry.Unregister(this);
    }
    protected virtual void Update()
    {
        if (upperPoint == null || lowerPoint == null) return;

        Vector3 target = isAtTop ? upperPoint.position : lowerPoint.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public virtual void GoUp() => isAtTop = true;
    public virtual void GoDown() => isAtTop = false;

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

    public virtual void ResetToInitialState()
    {
        Debug.Log($"elevator initial position: {initialPosition}");

        isAtTop = startAtTop;
        transform.position = initialPosition;
    }
}

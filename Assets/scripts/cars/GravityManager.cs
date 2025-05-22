using UnityEngine;
using Vuforia;

[RequireComponent(typeof(Rigidbody))]
public class GravityManager : MonoBehaviour
{
    [Header("Referencias")]
    public Rigidbody carRB;
    private ObserverBehaviour observer;

    [Header("Gravedad")]
    public float gravityMagnitude = 9.81f;

    private bool isTracked = false;

    void Awake()
    {
        if (carRB == null) carRB = GetComponent<Rigidbody>();
        observer = GetComponentInParent<ObserverBehaviour>();
        SetPhysicsState(false);
    }

    void OnEnable()
    {
        if (observer != null)
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    void OnDisable()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    void OnTargetStatusChanged(ObserverBehaviour _, TargetStatus status)
    {
        isTracked = status.Status == Status.TRACKED ||
                    status.Status == Status.EXTENDED_TRACKED;

        SetPhysicsState(isTracked);
    }

    void SetPhysicsState(bool enable)
    {
        carRB.isKinematic = !enable;
        carRB.useGravity = false;
        if (!enable) {
            carRB.linearVelocity = Vector3.zero;
            carRB.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (!isTracked) return;

        Vector3 customGravity = -observer.transform.up * gravityMagnitude;

        carRB.AddForce(customGravity, ForceMode.Acceleration);
    }
}

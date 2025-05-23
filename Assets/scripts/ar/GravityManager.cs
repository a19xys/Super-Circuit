using UnityEngine;
using Vuforia;

[RequireComponent(typeof(Rigidbody))]
public class GravityManager : MonoBehaviour
{
    public Rigidbody car;
    private ObserverBehaviour observer;
    public float gravityMagnitude = 9.81f;

    private bool isTracked = false;

    void Awake()
    {
        if (car == null) car = GetComponent<Rigidbody>();
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
        car.isKinematic = !enable;
        car.useGravity = false;
    }

    void FixedUpdate()
    {
        if (!isTracked) return;

        Vector3 customGravity = -observer.transform.up * gravityMagnitude;

        car.AddForce(customGravity, ForceMode.Acceleration);
    }
}

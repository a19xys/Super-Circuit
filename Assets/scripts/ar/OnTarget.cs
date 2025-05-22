using UnityEngine;

public class OnTarget : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField] private Transform targetTransform;
    private Vector3 respawnPosition;
    private float fallThresholdY = -10.0f;

    void Awake() {
        rb = GetComponent<Rigidbody>();

        if (targetTransform == null) {
            Debug.LogError("Debes asignar el Target en el Inspector.");
            return;
        }
        respawnPosition = targetTransform.InverseTransformPoint(transform.position);
    }

    void Update() {
        if (transform.position.y <= fallThresholdY) {
            Debug.Log("El objeto ha caído. Reiniciando posición...");
            ResetObject();
        }
    }

    public void enableRigidBody(bool enable) {
        Debug.Log("enableRigidBody " + enable);
        rb.constraints = enable ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
    }

    private void ResetObject() {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = targetTransform.TransformPoint(respawnPosition);
    }

}
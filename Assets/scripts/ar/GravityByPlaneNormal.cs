using UnityEngine;

public class GravityFromPlane : MonoBehaviour
{
    public Rigidbody car;
    public Transform gravityNormalReference; // Pon aquí el objeto hijo del plano
    public float gravityMagnitude = 0.2f;    // Ajusta para miniatura

    void FixedUpdate()
    {
        if (gravityNormalReference == null || car == null) return;
        Vector3 gravity = -gravityNormalReference.up * gravityMagnitude;
        car.AddForce(gravity, ForceMode.Acceleration);
    }
}

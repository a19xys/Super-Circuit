using UnityEngine;

public class RefillTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            FuelSystem fuelSystem = FindFirstObjectByType<FuelSystem>();
            if (fuelSystem != null) { fuelSystem.IniciarRecarga(); }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            FuelSystem fuelSystem = FindFirstObjectByType<FuelSystem>();
            if (fuelSystem != null) { fuelSystem.DetenerRecarga(); }
        }
    }

}
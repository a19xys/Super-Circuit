using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {

    public CheckpointManager manager;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) { manager.CheckpointTocado(gameObject); }
    }

}
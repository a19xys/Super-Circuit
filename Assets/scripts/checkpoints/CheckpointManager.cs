using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

    [Header("Checkpoints")]
    public List<GameObject> checkpoints;

    [Header("Vueltas")]
    public TextMeshProUGUI vueltasTexto;
    public int vueltasParaGanar = 3;

    [Header("Materiales")]
    public Material materialActual;
    public Material materialSiguiente;
    public Material materialFinal;

    private int checkpointActual = 0;
    private int vueltasCompletadas = 0;
    private bool carreraFinalizada = false;

    void Start() {
        ActivarCheckpoints();
        ActualizarTextoVueltas();
    }

    public void CheckpointTocado(GameObject checkpoint) {
        if (carreraFinalizada) return;
        if (checkpoint != checkpoints[checkpointActual]) return;

        // Desactiva el checkpoint actual (verde)
        checkpoint.SetActive(false);
        checkpointActual++;

        // Si se ha completado una vuelta
        if (checkpointActual >= checkpoints.Count) {
            vueltasCompletadas++;
            checkpointActual = 0;

            ActualizarTextoVueltas();

            if (vueltasCompletadas >= vueltasParaGanar) {
                carreraFinalizada = true;
                Debug.Log("GOAL!! Carrera finalizada");
                return;
            }
        }

        ActivarCheckpoints();
    }

    private void ActivarCheckpoints() {
        // Desactiva todos
        foreach (var cp in checkpoints) cp.SetActive(false);

        // Activa el actual como verde
        if (checkpointActual < checkpoints.Count) {
            var verde = checkpoints[checkpointActual];
            verde.SetActive(true);
            SetMaterial(verde, EsUltimoCheckpointFinal() ? materialFinal : materialActual);
        }

        // Activa el siguiente como azul
        int siguienteIndex = checkpointActual + 1;
        if (siguienteIndex < checkpoints.Count && !EsUltimaVuelta()) {
            var azul = checkpoints[siguienteIndex];
            azul.SetActive(true);
            SetMaterial(azul, materialSiguiente);
        }
    }

    private void SetMaterial(GameObject obj, Material mat) {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null) rend.material = mat;
    }

    private void ActualizarTextoVueltas() {
        if (vueltasCompletadas >= vueltasParaGanar) { vueltasTexto.text = "GOAL!!"; }
        else { vueltasTexto.text = $"{vueltasCompletadas + 1}/{vueltasParaGanar}"; }
    }

    private bool EsUltimaVuelta() {
        return vueltasCompletadas == vueltasParaGanar - 1;
    }

    private bool EsUltimoCheckpointFinal() {
        return EsUltimaVuelta() && checkpointActual == checkpoints.Count - 1;
    }
}
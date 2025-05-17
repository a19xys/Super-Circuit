using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

    [Header("Checkpoints")]
    public List<GameObject> vuelta1;
    public List<GameObject> vuelta2;
    public List<GameObject> vuelta3;

    [Header("Vueltas")]
    public TextMeshProUGUI vueltasTexto;
    public int vueltasParaGanar = 3;

    [Header("Materiales")]
    public Material materialActual;
    public Material materialSiguiente;
    public Material materialFinal;

    private List<List<GameObject>> recorridoPorVuelta;
    private int vueltasCompletadas = 0;
    private int checkpointActual = 0;
    private bool carreraFinalizada = false;

    void Start() {
        recorridoPorVuelta = new List<List<GameObject>> { vuelta1, vuelta2, vuelta3 };
        ActivarCheckpoints();
        ActualizarTextoVueltas();
    }

    public void CheckpointTocado(GameObject checkpoint) {
        if (carreraFinalizada) return;

        List<GameObject> recorrido = recorridoPorVuelta[vueltasCompletadas];

        if (checkpoint != recorrido[checkpointActual]) return;

        checkpoint.SetActive(false);
        checkpointActual++;

        if (checkpointActual >= recorrido.Count) {
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
        foreach (var vuelta in recorridoPorVuelta) {
            foreach (var cp in vuelta) {
                if (cp != null) cp.SetActive(false);
            }
        }

        List<GameObject> recorrido = recorridoPorVuelta[vueltasCompletadas];

        if (checkpointActual < recorrido.Count) {
            GameObject actual = recorrido[checkpointActual];
            actual.SetActive(true);

            CheckpointEffect efecto = actual.GetComponent<CheckpointEffect>();
            if (efecto != null) efecto.ResetVisual();

            CheckpointTrigger trigger = actual.GetComponent<CheckpointTrigger>();
            if (trigger != null) trigger.PrepararComoActual();

            bool esUltimoCheckpointFinal = EsUltimaVuelta() && checkpointActual == recorrido.Count - 1;
            SetMaterial(actual, esUltimoCheckpointFinal ? materialFinal : materialActual);
        }

        if (!EsUltimoCheckpointFinal()) {
            GameObject siguiente = ObtenerSiguienteCheckpoint();
            if (siguiente != null) {
                siguiente.SetActive(true);

                var efecto = siguiente.GetComponent<CheckpointEffect>();
                if (efecto != null) efecto.ResetVisual();

                var trigger = siguiente.GetComponent<CheckpointTrigger>();
                if (trigger != null) trigger.PrepararComoSiguiente();

                SetMaterial(siguiente, materialSiguiente);
            }
        }

    }

    private void SetMaterial(GameObject obj, Material mat) {
        Renderer rend = obj.GetComponent<Renderer>();

        if (rend != null) { rend.material = mat; }

        ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem>();
        
        if (ps != null) {
            var main = ps.main;
            main.startColor = mat.color;
        }

        var trigger = obj.GetComponent<CheckpointTrigger>();

        if (trigger != null) { trigger.EstablecerColorBase(mat.color); }
    }

    private void ActualizarTextoVueltas() {
        if (vueltasCompletadas >= vueltasParaGanar) { vueltasTexto.text = "GOAL!!"; }
        else { vueltasTexto.text = $"{vueltasCompletadas + 1}/{vueltasParaGanar}"; }
    }

    public bool EsUltimaVuelta() {
        return vueltasCompletadas == vueltasParaGanar - 1;
    }

    private bool EsUltimoCheckpointFinal() {
        if (!EsUltimaVuelta()) return false;
        List<GameObject> recorrido = recorridoPorVuelta[vueltasCompletadas];
        return checkpointActual == recorrido.Count - 1;
    }

    private GameObject ObtenerSiguienteCheckpoint() {
        List<GameObject> recorridoActual = recorridoPorVuelta[vueltasCompletadas];
        int siguienteIndex = checkpointActual + 1;

        if (siguienteIndex < recorridoActual.Count) { return recorridoActual[siguienteIndex]; }

        int siguienteVuelta = vueltasCompletadas + 1;

        if (siguienteVuelta < recorridoPorVuelta.Count) {
            List<GameObject> siguienteRecorrido = recorridoPorVuelta[siguienteVuelta];
            if (siguienteRecorrido.Count > 0) { return siguienteRecorrido[0]; }
        }

        return null;
    }

    public bool EsUltimoCheckpointDelRecorrido(GameObject checkpoint) {
        if (!EsUltimaVuelta()) return false;
        List<GameObject> recorrido = recorridoPorVuelta[vueltasCompletadas];
        return checkpoint == recorrido[recorrido.Count - 1];
    }

}
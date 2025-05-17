using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour {

    public CheckpointManager manager;
    public AudioClip sonidoError;
    public Material materialError;
    public float duracionParpadeo = 0.6f;

    [HideInInspector] public bool activable = false;

    private bool yaActivado = false;
    private AudioSource sfxSource;
    private Coroutine feedbackErrorCoroutine;
    private Renderer rend;
    private Material materialInstanciado;
    private Color colorActual;
    private ParticleSystem particles;

    void Start() {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        Renderer rend = GetComponent<Renderer>();

        if (rend != null) {
            materialInstanciado = rend.material;
            colorActual = materialInstanciado.color;
        }

        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        if (!activable) {
            if (sonidoError != null && sfxSource != null && !sfxSource.isPlaying) {
                sfxSource.volume = 1.5f;
                sfxSource.PlayOneShot(sonidoError);
            }

            if (feedbackErrorCoroutine == null) { feedbackErrorCoroutine = StartCoroutine(ParpadeoColorError()); }

            return;
        }

        if (yaActivado) return;
        yaActivado = true;

        bool esFinal = manager.EsUltimaVuelta() && manager.EsUltimoCheckpointDelRecorrido(this.gameObject);

        CheckpointEffect efecto = GetComponent<CheckpointEffect>();
        if (efecto != null) { efecto.ReproducirEfecto(() => { manager.CheckpointTocado(gameObject); }, esFinal); }
        else { manager.CheckpointTocado(gameObject); }
    }

    public void PrepararComoActual() {
        activable = true;
        yaActivado = false;
    }

    public void PrepararComoSiguiente() {
        activable = false;
        yaActivado = false;
    }

    private IEnumerator ParpadeoColorError() {
        if (materialInstanciado == null || materialError == null) { feedbackErrorCoroutine = null; yield break; }

        Renderer rend = GetComponent<Renderer>();
        rend.material = materialError;
        if (particles != null) {
            var main = particles.main;
            main.startColor = Color.red;
        }

        yield return new WaitForSeconds(duracionParpadeo);

        rend.material = materialInstanciado;
        if (particles != null) {
            var main = particles.main;
            main.startColor = colorActual;
        }

        feedbackErrorCoroutine = null;
    }

    public void EstablecerColorBase(Color nuevoColor) {
        colorActual = nuevoColor;

        Renderer rend = GetComponent<Renderer>();
        if (rend != null) {
            materialInstanciado = rend.material;
            materialInstanciado.color = nuevoColor;
        }
        if (particles != null) {
            var main = particles.main;
            main.startColor = nuevoColor;
        }
    }

}
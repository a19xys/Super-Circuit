using UnityEngine;

public class CheckpointEffect : MonoBehaviour {

    public AudioClip sonidoActivacion;
    public AudioClip sonidoVictoria;
    public float duracionAnimacion = 0.5f;

    private AudioSource audioSource;
    private Vector3 escalaInicial;
    private Quaternion rotacionInicial;

    void Start() {
        escalaInicial = transform.localScale;
        rotacionInicial = transform.rotation;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void ReproducirEfecto(System.Action onComplete, bool esFinal = false) {
        StartCoroutine(EfectoAnimado(onComplete, esFinal));
    }

    public void ResetVisual() {
        transform.localScale = escalaInicial;
        transform.rotation = rotacionInicial;
    }

    private System.Collections.IEnumerator EfectoAnimado(System.Action onFinish, bool esFinal) {
        if (esFinal && sonidoVictoria != null) { FanfareController.Instance.ReproducirClip(sonidoVictoria); }
        else if (sonidoActivacion != null) { audioSource.PlayOneShot(sonidoActivacion); }

        float tiempo = 0f;
        Vector3 escalaFinal = Vector3.zero;

        while (tiempo < duracionAnimacion) {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionAnimacion;

            transform.Rotate(720f * Time.deltaTime, 0, 0);
            transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);
            yield return null;
        }

        onFinish?.Invoke();
    }

}
using UnityEngine;

public class FanfareController : MonoBehaviour {

    public static FanfareController Instance;

    private AudioSource audioSource;

    void Awake() {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirClip(AudioClip clip) {
        if (clip != null) {
            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

}
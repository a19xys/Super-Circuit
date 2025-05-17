using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarAudioController : MonoBehaviour {

    [Header("Referencias")]
    public CarController carController;

    [Header("Clips")]
    public AudioClip engineClip;
    public AudioClip brakeScreechClip;

    private AudioSource motorSource;
    private AudioSource sfxSource;

    private bool estabaFrenando = false;

    void Start() {
        motorSource = gameObject.AddComponent<AudioSource>();
        motorSource.loop = true;
        motorSource.clip = engineClip;
        motorSource.pitch = 1.0f;
        motorSource.volume = 0.7f;
        motorSource.Play();

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = 0.6f;
    }

    void Update() {
        if (carController == null) { return; }

        if (!carController.MotorActivo()) {
            motorSource.pitch = 1.0f;
            motorSource.volume = 0.3f;
            estabaFrenando = false;
            return;
        }

        float vertical = Mathf.Abs(carController.GetVerticalInput());
        bool estaFrenando = carController.IsBraking();

        float targetPitch = Mathf.Lerp(1.0f, 2.0f, vertical);
        motorSource.pitch = Mathf.Lerp(motorSource.pitch, targetPitch, Time.deltaTime * 5f);

        if (estaFrenando && !estabaFrenando && !sfxSource.isPlaying && motorSource.pitch > 1.05f) { sfxSource.PlayOneShot(brakeScreechClip); }

        estabaFrenando = estaFrenando;
    }
}
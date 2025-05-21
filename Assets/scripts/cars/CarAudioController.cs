using UnityEngine;

public class CarAudioController : MonoBehaviour
{

    public CarController carController;

    [Header("Clips")]
    public AudioClip engineClip;
    public AudioClip brakeScreechClip;

    [Header("Sonido")]
    public float minBrakeScreechSpeed = 3f;

    private AudioSource motorSource;
    private AudioSource sfxSource;

    private bool estabaFrenando = false;

    void Start()
    {
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

    void Update()
    {
        if (carController == null) return;

        Rigidbody rb = carController.GetComponent<Rigidbody>();
        float speed = rb != null ? rb.linearVelocity.magnitude : 0f;

        // MOTOR
        if (!carController.MotorActivo())
        {
            if (motorSource.isPlaying)
                motorSource.Stop();
            estabaFrenando = false;
            return;
        }
        else
        {
            if (!motorSource.isPlaying)
                motorSource.Play();
        }

        float vertical = Mathf.Abs(carController.GetVerticalInput());
        bool estaFrenando = carController.IsBraking();

        float targetPitch = Mathf.Lerp(1.0f, 2.0f, vertical);
        motorSource.pitch = Mathf.Lerp(motorSource.pitch, targetPitch, Time.deltaTime * 5f);
        motorSource.volume = 0.7f;

        // MOTOR
        if (estaFrenando && !estabaFrenando && speed > minBrakeScreechSpeed && !sfxSource.isPlaying) { sfxSource.PlayOneShot(brakeScreechClip); }

        estabaFrenando = estaFrenando;
    }

    public void SetCarController(CarController nuevoCar)
    {
        carController = nuevoCar;
    }

}
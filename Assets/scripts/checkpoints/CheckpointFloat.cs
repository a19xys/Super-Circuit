using UnityEngine;

public class CheckpointFloat : MonoBehaviour {

    public float altura = 0.2f;
    public float velocidad = 2f;
    public float rotacionSpeed = 30f;

    private Vector3 posicionInicial;

    void Start() {
        posicionInicial = transform.position;
    }

    void Update() {
        float nuevaY = Mathf.Sin(Time.time * velocidad) * altura;
        transform.position = posicionInicial + new Vector3(0, nuevaY, 0);
    }
}
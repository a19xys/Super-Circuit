using UnityEngine;

public class CheckpointFloat : MonoBehaviour {

    public float altura = 0.2f;
    public float velocidad = 2f;
    public float rotacionSpeed = 30f;
    public Vector3 planoNormal = Vector3.up;

    private Vector3 posicionInicial;

    void Start() {
        posicionInicial = transform.position;
    }

    void Update() {
        float offset = Mathf.Sin(Time.time * velocidad) * altura;
        transform.position = posicionInicial + (planoNormal.normalized * offset);
    }

}

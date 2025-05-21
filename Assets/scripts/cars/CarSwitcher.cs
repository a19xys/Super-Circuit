using UnityEngine;
using UnityEngine.UI;

public class CarSwitcher : MonoBehaviour
{
    public CarAudioController audioController;
    public FuelSystem fuelSystemMorado;
    public FuelSystem fuelSystemRojo;

    public GameObject carMorado;
    public GameObject carRojo;
    public Transform puntoAparicion;

    [Header("Interfaz de coches")]
    public Button cambiarCocheButton;
    public Color colorBotonRojo;
    public Color colorBotonMorado;
    public GameObject sliderMorado;
    public GameObject sliderRojo;

    private GameObject cocheActual;

    private Vector3 ultimaPosicionMorado;
    private Quaternion ultimaRotacionMorado;

    private Vector3 ultimaPosicionRojo;
    private Quaternion ultimaRotacionRojo;

    void Start()
    {
        carMorado.SetActive(true);
        carRojo.SetActive(false);
        cocheActual = carMorado;

        ultimaPosicionMorado = puntoAparicion.position;
        ultimaRotacionMorado = puntoAparicion.rotation;
        ultimaPosicionRojo = puntoAparicion.position;
        ultimaRotacionRojo = puntoAparicion.rotation;

        ColocarCocheEnPunto(carMorado, ultimaPosicionMorado, ultimaRotacionMorado);

        sliderMorado.SetActive(true);
        sliderRojo.SetActive(false);
    }

    public void CambiarCoche()
    {
        if (cocheActual == carMorado)
        {
            ultimaPosicionMorado = carMorado.transform.position;
            ultimaRotacionMorado = carMorado.transform.rotation;

            if (fuelSystemMorado != null) fuelSystemMorado.ResetSinGasolinaTexto();

            carMorado.SetActive(false);

            cocheActual = carRojo;
            carRojo.SetActive(true);
            ColocarCocheEnPunto(carRojo, ultimaPosicionRojo, ultimaRotacionRojo);

            sliderMorado.SetActive(false);
            sliderRojo.SetActive(true);
        }
        else
        {
            ultimaPosicionRojo = carRojo.transform.position;
            ultimaRotacionRojo = carRojo.transform.rotation;

            if (fuelSystemRojo != null) fuelSystemRojo.ResetSinGasolinaTexto();

            carRojo.SetActive(false);

            cocheActual = carMorado;
            carMorado.SetActive(true);
            ColocarCocheEnPunto(carMorado, ultimaPosicionMorado, ultimaRotacionMorado);

            sliderMorado.SetActive(true);
            sliderRojo.SetActive(false);
        }

        CarController nuevoCarController = cocheActual.GetComponent<CarController>();
        if (audioController != null) audioController.SetCarController(nuevoCarController);
        ActualizarColorBoton();
    }

    private void ColocarCocheEnPunto(GameObject coche, Vector3 pos, Quaternion rot)
    {
        coche.transform.position = pos;
        coche.transform.rotation = rot;
        ResetVelocity(coche);
    }

    private void ResetVelocity(GameObject coche)
    {
        Rigidbody rb = coche.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void ActualizarColorBoton()
    {
        if (cambiarCocheButton == null) return;
        var img = cambiarCocheButton.GetComponent<Image>();
        if (img == null) return;

        if (cocheActual == carMorado) { img.color = colorBotonRojo; }
        else { img.color = colorBotonMorado; }
    }

    public GameObject GetCocheActual() => cocheActual;
    public CarController GetCarControllerActual() => cocheActual.GetComponent<CarController>();
}
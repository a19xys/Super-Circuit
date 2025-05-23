using UnityEngine;
using Vuforia;

public class RemoteConnectionManager : MonoBehaviour
{
    [Header("Targets")]
    public GameObject controlTarget;
    public GameObject carTarget;

    [Header("Parámetros")]
    public float distanciaMaxima = 0.25f;

    [Header("Control")]
    public CarSwitcher carSwitcher;

    [Header("UI")]
    public GameObject avisoSinConexion;
    public GameObject avisoConConexion;

    private bool conectado = true;

    void Start()
    {
        SetConexion(false);
    }

    void Update()
    {
        var ctrlObs = controlTarget.GetComponent<ObserverBehaviour>();
        var carObs = carTarget.GetComponent<ObserverBehaviour>();

        bool ctrlTracked = ctrlObs != null &&
          (ctrlObs.TargetStatus.Status == Status.TRACKED || ctrlObs.TargetStatus.Status == Status.EXTENDED_TRACKED);
        bool carTracked = carObs != null &&
          (carObs.TargetStatus.Status == Status.TRACKED || carObs.TargetStatus.Status == Status.EXTENDED_TRACKED);

        if (!ctrlTracked || !carTracked)
        {
            SetConexion(false);
            return;
        }

        float distancia = Vector3.Distance(controlTarget.transform.position, carTarget.transform.position);
        bool nuevaConexion = distancia <= distanciaMaxima;
        SetConexion(nuevaConexion);
    }

    private void SetConexion(bool estado)
    {
        if (conectado == estado) return;

        conectado = estado;

        var car = carSwitcher.GetCarControllerActual();
        if (car != null) car.SetMotorActivo(conectado);

        if (conectado) { MostrarConSenal(); }
        else { MostrarSinSenal(); }
    }

    void MostrarSinSenal()
    {
        if (avisoSinConexion != null)
            avisoSinConexion.SetActive(true);
        if (avisoConConexion != null)
            avisoConConexion.SetActive(false);
    }

    void MostrarConSenal()
    {
        if (avisoSinConexion != null)
            avisoSinConexion.SetActive(false);
        if (avisoConConexion != null)
            avisoConConexion.SetActive(true);
    }
}
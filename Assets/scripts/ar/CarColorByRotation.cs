using UnityEngine;
using Vuforia;

public class CarSwitchByRotation : MonoBehaviour
{
    public CarSwitcher carSwitcher; // Arrastra aquí el objeto con tu CarSwitcher

    private ObserverBehaviour observerBehaviour;
    private int ultimoSector = -1;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
    }

    void Update()
    {
        // Requiere tracking (no uses .IsTracked)
        if (observerBehaviour == null ||
            (observerBehaviour.TargetStatus.Status != Status.TRACKED &&
             observerBehaviour.TargetStatus.Status != Status.EXTENDED_TRACKED))
        {
            return;
        }

        float yRotation = transform.eulerAngles.y % 360f;
        if (yRotation < 0) yRotation += 360f;

        // Divide la circunferencia en dos: [0,180) = morado, [180,360) = rojo
        int sector = yRotation < 180f ? 0 : 1;

        // Si cambiamos de sector, cambiamos de coche
        if (sector != ultimoSector)
        {
            bool moradoActivo = carSwitcher.GetCocheActual() == carSwitcher.carMorado;

            // Sector 0: debería estar el morado activo
            // Sector 1: debería estar el rojo activo
            if ((sector == 0 && !moradoActivo) || (sector == 1 && moradoActivo))
            {
                carSwitcher.CambiarCoche();
            }
            ultimoSector = sector;
        }
    }
}

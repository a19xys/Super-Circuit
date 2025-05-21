using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FuelSystem : MonoBehaviour
{

    [Header("Dependencias")]
    public CarController carController;
    public Slider fuelBar;

    [Header("Configuración")]
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 5f;
    public float refillRate = 20f;
    private bool enZonaDeRecarga = false;

    [Header("Aviso gasolina")]
    public TextMeshProUGUI sinGasolinaTexto;
    public float velocidadParpadeo = 1f;

    private Coroutine parpadeoCoroutine;
    private float currentFuel;
    private bool isOutOfFuel = false;

    void Start()
    {
        currentFuel = maxFuel;
        UpdateFuelUI();
    }

    void Update()
    {
        // --- Repostaje ---
        if (enZonaDeRecarga && currentFuel < maxFuel)
        {
            currentFuel += refillRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
            UpdateFuelUI();

            if (isOutOfFuel && currentFuel > 1f)
            {
                isOutOfFuel = false;
                carController.SetMotorActivo(true);
                DesactivarAvisoSinGasolina();
            }
            
            return;
        }

        // --- Consumo de gasolina ---
        if (isOutOfFuel) return;

        float speed = carController.GetComponent<Rigidbody>().linearVelocity.magnitude;

        if (speed > 0.1f)
        {
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
            UpdateFuelUI();

            if (currentFuel <= 0f)
            {
                isOutOfFuel = true;
                carController.SetMotorActivo(false);
                ActivarAvisoSinGasolina();
            }
        }
    }

    public void RefillFuel()
    {
        currentFuel = maxFuel;
        isOutOfFuel = false;
        UpdateFuelUI();
        carController.SetMotorActivo(true);
    }

    private void UpdateFuelUI()
    {
        if (fuelBar != null) { fuelBar.value = currentFuel / maxFuel; }
    }

    private void ActivarAvisoSinGasolina()
    {
        if (sinGasolinaTexto == null) return;

        sinGasolinaTexto.gameObject.SetActive(true);

        if (parpadeoCoroutine == null)
            parpadeoCoroutine = StartCoroutine(ParpadeoTexto());
    }

    private void DesactivarAvisoSinGasolina()
    {
        if (sinGasolinaTexto == null) return;

        sinGasolinaTexto.gameObject.SetActive(false);

        if (parpadeoCoroutine != null)
        {
            StopCoroutine(parpadeoCoroutine);
            parpadeoCoroutine = null;
        }
    }

    private IEnumerator ParpadeoTexto()
    {
        while (true)
        {
            float alpha = Mathf.PingPong(Time.time * velocidadParpadeo, 1f);
            Color color = sinGasolinaTexto.color;
            color.a = alpha;
            sinGasolinaTexto.color = color;
            yield return null;
        }
    }

    public void ResetSinGasolinaTexto()
    {
        if (sinGasolinaTexto == null) return;

        sinGasolinaTexto.gameObject.SetActive(false);

        if (parpadeoCoroutine != null)
        {
            StopCoroutine(parpadeoCoroutine);
            parpadeoCoroutine = null;
        }
        Color c = sinGasolinaTexto.color;
        c.a = 1f;
        sinGasolinaTexto.color = c;
    }

    void OnEnable()
    {
        if (isOutOfFuel && sinGasolinaTexto != null)
        {
            Color c = sinGasolinaTexto.color;
            c.a = 1f;
            sinGasolinaTexto.color = c;
            sinGasolinaTexto.gameObject.SetActive(true);

            if (parpadeoCoroutine != null) StopCoroutine(parpadeoCoroutine);
            parpadeoCoroutine = StartCoroutine(ParpadeoTexto());
        }
    }

    public void IniciarRecarga() => enZonaDeRecarga = true;
    public void DetenerRecarga() => enZonaDeRecarga = false;

}
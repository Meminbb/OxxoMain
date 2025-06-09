using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ActivadorAleatorio : MonoBehaviour
{
    public GameObject objetoAActivar;
    public Image barraImagen;
    public AudioSource sonidoAlerta;
    public AudioSource sonidoMalo;
    public TextMeshProUGUI textoDescuento;

    private float duracionActiva = 15f;

    void Start()
    {
        StartCoroutine(ActivarYDesactivarObjeto());
    }

    IEnumerator QuitarMonedas(int idUsuario)
    {
        string url = $"https://10.22.221.26:5001/api/GabrielMartinez/quitar-monedas/{idUsuario}";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "");
        request.certificateHandler = new ForceAcceptAll();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al quitar monedas: " + request.error);
        }
        else
        {
            Debug.Log("Monedas quitadas");
        }
    }

    IEnumerator MostrarTextoDescuento()
    {
        textoDescuento.gameObject.SetActive(true);
        textoDescuento.text = "-5";

        Color colorInicial = textoDescuento.color;
        colorInicial.a = 1f;
        textoDescuento.color = colorInicial;

        float duracion = 5f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracion);
            textoDescuento.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            yield return null;
        }

        textoDescuento.gameObject.SetActive(false);
    }

    IEnumerator ActivarYDesactivarObjeto()
    {
        while (true)
        {
            int idUsuario = PlayerPrefs.GetInt("idUsuario", -1);
            float tiempoEspera = Random.Range(25f, 35f);
            yield return new WaitForSeconds(tiempoEspera);

            if (objetoAActivar != null)
            {
                objetoAActivar.SetActive(true);

                if (sonidoAlerta != null)
                {
                    sonidoAlerta.Play();
                }

                if (barraImagen != null)
                {
                    barraImagen.gameObject.SetActive(true);
                    barraImagen.fillAmount = 1f;

                    float tiempoRestante = duracionActiva;
                    bool sonidoFinalReproducido = false;

                    while (tiempoRestante > 0)
                    {
                        tiempoRestante -= Time.deltaTime;

                        float porcentaje = tiempoRestante / duracionActiva;
                        barraImagen.fillAmount = porcentaje;
                        barraImagen.color = Color.Lerp(Color.red, Color.green, porcentaje);

                        if (!sonidoFinalReproducido && tiempoRestante <= 2f)
                        {
                            if (sonidoMalo != null) sonidoMalo.Play();
                            sonidoFinalReproducido = true;
                        }

                        yield return null;
                    }

                    barraImagen.fillAmount = 0f;
                    barraImagen.gameObject.SetActive(false);
                }
                else
                {
                    yield return new WaitForSeconds(duracionActiva);
                }

                objetoAActivar.SetActive(false);

                yield return StartCoroutine(QuitarMonedas(idUsuario));
                StartCoroutine(MostrarTextoDescuento());

                MostrarMonedas mostrar = FindAnyObjectByType<MostrarMonedas>();
                if (mostrar == null)
                {
                    Debug.LogWarning("⚠️ No se encontró MostrarMonedas en escena. Asegúrate de tenerlo en el HUD activo.");
                }
                else
                {
                    mostrar.ActualizarMonedasDesdeOtroScript();
                }

                FindFirstObjectByType<CargarDesdeMain>()?.ActualizarPromocion();
            }
        }
    }

}

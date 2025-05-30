using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ActivadorCiclo : MonoBehaviour
{
    public GameObject objetoAReubicar;
    public Transform[] puntosDeAparicion;
    public Image barraImagen;
    public AudioSource sonidoAlerta;
    public AudioSource sonidoMalo;
    public float tiempoActivo = 15f;
    public TextMeshProUGUI textoDescuento; 

    void Start()
    {
        if (puntosDeAparicion.Length != 4)
        {
            Debug.LogError("Debes asignar exactamente 4 puntos de aparición.");
            return;
        }

        objetoAReubicar.SetActive(false);
        StartCoroutine(CicloActivacion());
    }
    IEnumerator MostrarTextoDescuento()
    {
        textoDescuento.gameObject.SetActive(true);
        textoDescuento.text = "-5 ";

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

        IEnumerator QuitarMonedas(int idUsuario)
    {
        
        string url = $"https://10.22.165.130:7275/api/GabrielMartinez/quitar-monedas/{idUsuario}";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "");
        request.certificateHandler = new ForceAcceptAll();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al quitar monedas: " + request.error);
        }
        else
        {
            Debug.Log("monedas quitadas");
        }
    }

    IEnumerator CicloActivacion()
    {
        while (true)
        {
            int idUsuario = PlayerPrefs.GetInt("idUsuario", -1);
            float tiempoEspera = Random.Range(30f, 60f);
            yield return new WaitForSeconds(tiempoEspera);

            int indice = Random.Range(0, puntosDeAparicion.Length);
            objetoAReubicar.transform.position = puntosDeAparicion[indice].position;
            objetoAReubicar.SetActive(true);

            if (sonidoAlerta != null) sonidoAlerta.Play();

            if (barraImagen != null)
            {
                barraImagen.gameObject.SetActive(true);
                barraImagen.fillAmount = 1f;

                float tiempoRestante = tiempoActivo;
                while (tiempoRestante > 0)
                {
                    tiempoRestante -= Time.deltaTime;
                    float porcentaje = tiempoRestante / tiempoActivo;
                    barraImagen.fillAmount = porcentaje;
                    barraImagen.color = Color.Lerp(Color.red, Color.green, porcentaje);
                    yield return null;
                }

                barraImagen.fillAmount = 0f;
                barraImagen.gameObject.SetActive(false);
            }
            else
            {
                yield return new WaitForSeconds(tiempoActivo);
            }

            objetoAReubicar.SetActive(false);
            if (sonidoMalo != null) sonidoMalo.Play();
            yield return StartCoroutine(QuitarMonedas(idUsuario));
            StartCoroutine(MostrarTextoDescuento());


            MostrarMonedas mostrar = FindAnyObjectByType<MostrarMonedas>();
            if (mostrar != null)
            {
                mostrar.ActualizarMonedasDesdeOtroScript();
            }

            FindFirstObjectByType<CargarDesdeMain>().ActualizarPromocion();


        }
    }
}

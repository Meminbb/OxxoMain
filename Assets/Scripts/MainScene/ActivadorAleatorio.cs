using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivadorAleatorio : MonoBehaviour
{
    public GameObject objetoAActivar;
    public Image barraImagen;
    public AudioSource sonidoAlerta; // ← Asigna el AudioSource desde el Inspector
    public AudioSource sonidoMalo;

    private float duracionActiva = 15f;

    void Start()
    {
        StartCoroutine(ActivarYDesactivarObjeto());
    }

    IEnumerator ActivarYDesactivarObjeto()
    {
        while (true)
        {
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

                    while (tiempoRestante > 0)
                    {
                        tiempoRestante -= Time.deltaTime;

                        float porcentaje = tiempoRestante / duracionActiva;
                        barraImagen.fillAmount = porcentaje;
                        barraImagen.color = Color.Lerp(Color.red, Color.green, porcentaje);

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
                sonidoMalo.Play();
                // Al momento de activar el NPC
                FindFirstObjectByType<CargarDesdeMain>().ActualizarPromocion();

                
            }
        }
    }
}

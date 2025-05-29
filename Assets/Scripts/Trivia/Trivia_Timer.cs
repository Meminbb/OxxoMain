using UnityEngine;
using UnityEngine.UI;

public class Trivia_Timer : MonoBehaviour
{
    public float tiempoMaximo = 10f;
    private float tiempoRestante;

    public Image barraTiempo;
    public GameObject panelTimeOver;
    public Button[] botonesRespuesta;
    public Text mensajeResultado;  // ← mensaje principal
    public Text textoPuntaje;      // ← texto para puntaje

    private bool tiempoActivo = false;

    void Start()
    {
        ReiniciarTemporizador();
    }

    void Update()
    {
        if (!tiempoActivo) return;

        tiempoRestante -= Time.deltaTime;
        barraTiempo.fillAmount = tiempoRestante / tiempoMaximo;

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            tiempoActivo = false;
            TiempoTerminado();
        }
    }

    public void ReiniciarTemporizador()
    {
        tiempoRestante = tiempoMaximo;
        tiempoActivo = true;
        barraTiempo.fillAmount = 1f;

        foreach (Button btn in botonesRespuesta)
            btn.interactable = true;

        if (panelTimeOver != null)
            panelTimeOver.SetActive(false);

        if (textoPuntaje != null)
            textoPuntaje.text = ""; // limpiar puntaje anterior
    }

    public void DetenerTemporizador()
    {
        Debug.Log("🛑 Temporizador detenido correctamente");
        tiempoActivo = false;
    }

    void TiempoTerminado()
    {
        Debug.Log("⏰ Tiempo agotado");

        foreach (Button btn in botonesRespuesta)
            btn.interactable = false;

        if (panelTimeOver != null)
        {
            panelTimeOver.SetActive(true);

            if (mensajeResultado != null)
                mensajeResultado.text = "Tiempo acabado.";

            if (textoPuntaje != null)
                textoPuntaje.text = "Puntos: 0";
        }
    }

    public float ObtenerTiempoRestante()
    {
        return tiempoRestante;
    }
}

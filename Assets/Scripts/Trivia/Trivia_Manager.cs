using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Trivia_Manager : MonoBehaviour
{
    public Text preguntaText;
    public Button[] botonesRespuesta;
    public Text textoResultado;
    public Text textoPuntaje;

    public Trivia_Timer temporizador;

    public AudioSource audioCorrecto;   // ← Nuevo
    public AudioSource audioIncorrecto; // ← Nuevo

    private List<Trivia_Respuestas> respuestasActuales;
    private bool juegoFinalizado = false;

    IEnumerator Start()
    {
        yield return StartCoroutine(CargarPreguntaAleatoria());
    }

    IEnumerator CargarPreguntaAleatoria()
    {
        string JSONurl = "https://10.22.207.76:7172/Trivia/pregunta";
        UnityWebRequest web = UnityWebRequest.Get(JSONurl);
        web.certificateHandler = new Trivia_ForceAcceptAll();
        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error API (pregunta): " + web.error);
        }
        else
        {
            List<Trivia_Preguntas> preguntaLista = JsonConvert.DeserializeObject<List<Trivia_Preguntas>>(web.downloadHandler.text);
            if (preguntaLista.Count > 0)
            {
                Trivia_Preguntas pregunta = preguntaLista[0];
                preguntaText.text = pregunta.texto_pregunta;
                yield return StartCoroutine(CargarRespuestas(pregunta.id_pregunta));
            }
        }
    }

    IEnumerator CargarRespuestas(int idPregunta)
    {
        string JSONurl = $"https://10.22.207.76:7172/Trivia/pregunta/{idPregunta}/respuestas";
        UnityWebRequest web = UnityWebRequest.Get(JSONurl);
        web.certificateHandler = new Trivia_ForceAcceptAll();
        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error API (respuestas): " + web.error);
        }
        else
        {
            respuestasActuales = JsonConvert.DeserializeObject<List<Trivia_Respuestas>>(web.downloadHandler.text);

            for (int i = 0; i < botonesRespuesta.Length; i++)
            {
                if (i < respuestasActuales.Count)
                {
                    int index = i;
                    botonesRespuesta[i].GetComponentInChildren<Text>().text = respuestasActuales[i].texto_respuesta;
                    botonesRespuesta[i].onClick.RemoveAllListeners();
                    botonesRespuesta[i].onClick.AddListener(() => VerificarRespuesta(index));
                    botonesRespuesta[i].interactable = true;
                }
                else
                {
                    botonesRespuesta[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void VerificarRespuesta(int index)
    {
        if (juegoFinalizado) return;
        juegoFinalizado = true;

        bool esCorrecta = respuestasActuales[index].es_correcta == "1" || respuestasActuales[index].es_correcta.ToLower() == "true";

        for (int i = 0; i < botonesRespuesta.Length; i++)
        {
            Image btnImage = botonesRespuesta[i].GetComponent<Image>();

            if (i == index)
            {
                if (esCorrecta)
                {
                    Debug.Log("✅ Correcto");
                    btnImage.color = Color.green;
                    textoResultado.text = "¡Enhorabuena! Respuesta correcta";

                    int puntos = Mathf.RoundToInt(temporizador.ObtenerTiempoRestante() * 10f);
                    textoPuntaje.text = $"Puntos: {puntos}";

                    if (audioCorrecto != null) audioCorrecto.Play();  // ✅ Sonido correcto
                }
                else
                {
                    Debug.Log("❌ Incorrecto");
                    btnImage.color = Color.red;
                    textoResultado.text = "Respuesta incorrecta. Siga leyendo el manual.";
                    textoPuntaje.text = "Puntos: 0";

                    if (audioIncorrecto != null) audioIncorrecto.Play();  // ✅ Sonido incorrecto
                }
            }
            else
            {
                if (respuestasActuales[i].es_correcta == "1" || respuestasActuales[i].es_correcta.ToLower() == "true")
                    botonesRespuesta[i].GetComponent<Image>().color = new Color(0.6f, 1f, 0.6f);
                else
                    botonesRespuesta[i].GetComponent<Image>().color = new Color(1f, 0.6f, 0.6f);
            }

            botonesRespuesta[i].interactable = false;
        }

        temporizador.DetenerTemporizador();

        if (temporizador.panelTimeOver != null)
        {
            temporizador.panelTimeOver.SetActive(true);

            if (textoResultado != null)
                temporizador.mensajeResultado.text = textoResultado.text;

            if (textoPuntaje != null)
                temporizador.textoPuntaje.text = textoPuntaje.text;
        }
    }
}

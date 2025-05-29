using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    public int idUsuario = 1;
    public int puntajeTotal = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarPuntos(int puntos)
    {
        puntajeTotal += puntos;
        Debug.Log("🟢 Puntaje acumulado: " + puntajeTotal);
    }

    public void EnviarPuntosFinales()
    {
        Instance.StartCoroutine(EnviarPuntosAPI(puntajeTotal));
    }

    private IEnumerator EnviarPuntosAPI(int puntos)
    {
        string JSONurl = $"https://10.22.207.76:7172/Trivia/usuario/{idUsuario}/puntos";
        UnityWebRequest web = new UnityWebRequest(JSONurl, "POST");
        byte[] datos = System.Text.Encoding.UTF8.GetBytes(puntos.ToString());
        web.uploadHandler = new UploadHandlerRaw(datos);
        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.certificateHandler = new Trivia_ForceAcceptAll();

        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
            Debug.Log("❌ Error al enviar puntos: " + web.error);
        else
            Debug.Log("✅ Puntos enviados correctamente: " + web.downloadHandler.text);
    }
}

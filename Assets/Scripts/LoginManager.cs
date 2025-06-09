using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField inputUsuario;
    public TMP_InputField inputContrasena;
    public Button botonLogin;
    public TMP_Text textoEstado;

    public GameObject mensajeErrorObject;
    private CanvasGroup canvasGroupError;

    private const string loginURL = "https://192.168.1.78:5001/api/GabrielMartinez/login";

    void Start()
    {
        botonLogin.interactable = true;
        botonLogin.onClick.AddListener(() => StartCoroutine(IniciarSesion()));

        if (mensajeErrorObject != null)
        {
            canvasGroupError = mensajeErrorObject.GetComponent<CanvasGroup>();
            canvasGroupError.alpha = 0f;
            mensajeErrorObject.SetActive(false);
        }
    }

    IEnumerator IniciarSesion()
    {
        textoEstado.text = "Iniciando sesión...";

        // Validación inicial de campos vacíos
        if (string.IsNullOrWhiteSpace(inputUsuario.text) || string.IsNullOrWhiteSpace(inputContrasena.text))
        {
            yield return StartCoroutine(MostrarMensajeError("Favor de llenar todos los campos"));
            yield break;
        }

        LoginRequest datos = new LoginRequest
        {
            Usuario = inputUsuario.text,
            Contraseña = inputContrasena.text
        };

        string json = JsonUtility.ToJson(datos);
        UnityWebRequest request = new UnityWebRequest(loginURL);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.certificateHandler = new ForceAcceptAll();
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        string respuesta = request.downloadHandler.text;
        LoginResponse response = null;

        try
        {
            response = JsonUtility.FromJson<LoginResponse>(respuesta);
        }
        catch
        {
            response = null;
        }

        if (response == null || response.idUsuario <= 0)
        {
            yield return StartCoroutine(MostrarMensajeError("Iniciar sesión fallido"));
        }
        else
        {
            textoEstado.text = "Bienvenido. ID: " + response.idUsuario;

            PlayerPrefs.SetInt("idUsuario", response.idUsuario);
            PlayerPrefs.Save();

            SceneManager.LoadScene("MainScene");
        }
    }

    IEnumerator MostrarMensajeError(string mensaje)
    {
        textoEstado.text = mensaje;
        mensajeErrorObject.SetActive(true);

        float duracion = 0.5f;

        // Fade in
        for (float t = 0; t < duracion; t += Time.deltaTime)
        {
            canvasGroupError.alpha = t / duracion;
            yield return null;
        }
        canvasGroupError.alpha = 1f;

        yield return new WaitForSeconds(1.5f);

        // Fade out
        for (float t = 0; t < duracion; t += Time.deltaTime)
        {
            canvasGroupError.alpha = 1f - (t / duracion);
            yield return null;
        }
        canvasGroupError.alpha = 0f;

        mensajeErrorObject.SetActive(false);
    }

    [System.Serializable]
    public class LoginRequest
    {
        public string Usuario;
        public string Contraseña;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string mensaje;
        public int idUsuario;
    }

    private class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}

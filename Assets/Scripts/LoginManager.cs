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

    private const string loginURL = "https://10.22.165.130:7275/api/GabrielMartinez/login";

    void Start()
    {
        botonLogin.interactable = true;
        botonLogin.onClick.AddListener(() => StartCoroutine(IniciarSesion()));
    }

    IEnumerator IniciarSesion()
    {
        textoEstado.text = "Iniciando sesión...";

        // Validar campos vacíos antes de enviar
        if (string.IsNullOrWhiteSpace(inputUsuario.text) || string.IsNullOrWhiteSpace(inputContrasena.text))
        {
            textoEstado.text = "Por favor completa ambos campos.";
            yield break;
        }

        LoginRequest datos = new LoginRequest
        {
            Usuario = inputUsuario.text,
            Contraseña = inputContrasena.text
        };

        string json = JsonUtility.ToJson(datos);
        Debug.Log("JSON enviado: " + json);

        UnityWebRequest request = new UnityWebRequest(loginURL);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.certificateHandler = new ForceAcceptAll(); // Aceptar certificados inválidos
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            textoEstado.text = "Error: " + request.downloadHandler.text;
            botonLogin.interactable = true;
        }
        else
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            textoEstado.text = "Bienvenido. ID: " + response.idUsuario;

            PlayerPrefs.SetInt("idUsuario", response.idUsuario);
            PlayerPrefs.Save();

            SceneManager.LoadScene("MainScene"); 

            
        }
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

    // Forzar aceptar certificado (solo en desarrollo)
    private class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}

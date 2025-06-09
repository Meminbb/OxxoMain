using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class MostrarMonedas : MonoBehaviour
{
    public TextMeshProUGUI textoMonedas;
    public TextMeshProUGUI usuario;
    public TextMeshProUGUI score;
    private string baseUrl = "https://10.22.221.26:5001/api/GabrielMartinez/monedas/"; 

    void Start()
    {
        int idUsuario = PlayerPrefs.GetInt("idUsuario", -1);

        if (idUsuario != -1)
        {
            StartCoroutine(CargarMonedas(idUsuario));
        }
        else
        {
            textoMonedas.text = "Usuario no logueado";
        }
    }

    IEnumerator CargarMonedas(int idUsuario)
    {
        string url = baseUrl + idUsuario;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.certificateHandler = new ForceAcceptAll(); 
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            textoMonedas.text = "Error al obtener monedas";
            Debug.LogError(request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            MonedasResponse response = JsonUtility.FromJson<MonedasResponse>(json);
            textoMonedas.text = response.monedas.ToString();
            score.text = "Puntaje:" + response.score;
            usuario.text =  response.usuario.ToString();
        }
    }

    public void ActualizarMonedasDesdeOtroScript()
    {
        int idUsuario = PlayerPrefs.GetInt("idUsuario", -1);
        if (idUsuario != -1)
        {
            StartCoroutine(CargarMonedas(idUsuario));
        }
    }

    [System.Serializable]
    public class MonedasResponse
    {
        public int monedas;
        public int score;
        public string usuario;
    }

    private class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}

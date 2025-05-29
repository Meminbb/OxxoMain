using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class MostrarMonedas : MonoBehaviour
{
    public TextMeshProUGUI textoMonedas;
    private string apiUrl = "https://10.22.165.130:7275/api/GabrielMartinez/monedas/1"; 

    void Start()
    {
        StartCoroutine(CargarMonedas());
    }

    IEnumerator CargarMonedas()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
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
        }
    }

    [System.Serializable]
    public class MonedasResponse
    {
        public int monedas;
    }
}

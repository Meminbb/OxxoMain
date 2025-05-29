using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

public class CargarDesdeMain : MonoBehaviour
{
    public string apiURL = "https://10.22.171.217:7275/api/GabrielMartinez/promo-random";

    public Image imagenProductoPromo;
    public Image imagenPromo;
    public RectTransform contenedorPromos;
    public Vector2 posicionCentro = new Vector2(0, 0);
    public Vector2 posicionIzquierda = new Vector2(-960, 0);

    private bool estaEnCentro = false;

    public void AlternarPosicion()
    {

        contenedorPromos.anchoredPosition = estaEnCentro ? posicionIzquierda : posicionCentro;
        estaEnCentro = !estaEnCentro;
    }

    void Start()
    {
        StartCoroutine(ObtenerDatosDesdeAPI());
    }

    IEnumerator ObtenerDatosDesdeAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiURL);
        request.certificateHandler = new ForceAcceptAll();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error en API: " + request.error);
            yield break;
        }

        PedidoResponse pedido = JsonConvert.DeserializeObject<PedidoResponse>(request.downloadHandler.text);

        DatosJuego.PromocionSeleccionada = pedido.Promocion;
        DatosJuego.TodasLasPromociones = pedido.TodasLasPromociones;
        DatosJuego.ProductosSeleccionados = pedido.Productos;
        DatosJuego.PrecioTotalFinal = pedido.PrecioTotalFinal;

        Debug.Log("✅ Promoción y productos guardados para la escena Cajero");

        if (imagenProductoPromo != null)
            StartCoroutine(DownloadImage(pedido.Promocion.ImgProductoPromo, imagenProductoPromo));

        if (imagenPromo != null)
            StartCoroutine(DownloadImage(pedido.Promocion.ImgPromo, imagenPromo));
    }

    IEnumerator DownloadImage(string url, Image targetImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        request.certificateHandler = new ForceAcceptAll();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            targetImage.sprite = sprite;
            targetImage.color = Color.white;
        }
        else
        {
            Debug.LogWarning("⚠️ No se pudo cargar imagen desde: " + url);
        }
    }

    public class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
    public void IrACajero()
    {
        SceneManager.LoadScene("Cajero");
    }
    
    public void ActualizarPromocion()
    {
        StartCoroutine(ObtenerDatosDesdeAPI());
    }
}

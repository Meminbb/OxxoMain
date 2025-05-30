
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CargarPedido : MonoBehaviour
{
    [System.Serializable]
    public class ContenedorProducto
    {
        public Image imagen;
    }

    public ContenedorProducto[] contenedores = new ContenedorProducto[3];

    public AudioSource musicaFondo;
    public AudioSource audioVictoria;
    public AudioSource audioDerrota;

    public TextMeshProUGUI textoPrecioFinal;
    public TextMeshProUGUI textoDineroCliente;
    public TextMeshProUGUI textoCambioJugador;

    public Transform contenedorPromociones;
    public GameObject prefabOpcionPromo;
    public GameObject contenedorPromos;
    public GameObject pantallaResultado;
    public TextMeshProUGUI textoResultado;
    public Image barraTiempo;
    public SpriteRenderer npcRenderer;


    private float precioTotal = 0f;
    private float dineroCliente = 0f;
    private float cambioEsperado = 0f;
    private float cambioJugador = 0f;

    private int idPromocionCorrecta;
    private int idPromocionSeleccionada = -1;

    private Dictionary<float, bool> billetesSeleccionados = new Dictionary<float, bool>();
    private List<Button> botonesPromociones = new List<Button>();
    private List<Promocion> promocionesActuales = new List<Promocion>();

    private float tiempoLimite = 20f;
    private bool resultadoMostrado = false;

    void Start()
    {
        if (musicaFondo != null)
        {
            musicaFondo.volume = 1f;
            musicaFondo.pitch = 1f;
            musicaFondo.Play();
        }

        StartCoroutine(UsarDatosGuardados());
        StartCoroutine(TemporizadorCuentaRegresiva());
    }

    IEnumerator UsarDatosGuardados()
    {
        promocionesActuales = DatosJuego.TodasLasPromociones;
        idPromocionCorrecta = DatosJuego.PromocionSeleccionada.IdPromocion;

        for (int i = 0; i < DatosJuego.ProductosSeleccionados.Count && i < contenedores.Length; i++)
        {
            var p = DatosJuego.ProductosSeleccionados[i];
            yield return StartCoroutine(DownloadImage(p.Img, contenedores[i].imagen));
        }

        precioTotal = DatosJuego.PrecioTotalFinal;
        textoPrecioFinal.text = "$" + precioTotal.ToString("0.00");

        float extra = Random.Range(0.1f, 0.5f);
        dineroCliente = Mathf.Ceil(precioTotal * (1 + extra));
        int dineroEntero = Mathf.CeilToInt(dineroCliente);
        dineroCliente = dineroEntero;
        textoDineroCliente.text = "$" + dineroEntero.ToString();

        cambioEsperado = dineroCliente - precioTotal;
        cambioJugador = 0f;
        textoCambioJugador.text = "$0";

        foreach (Transform child in contenedorPromociones)
            Destroy(child.gameObject);

        botonesPromociones.Clear();

        float spacing = 270f;
        float startX = -540f;
        int index = 0;

        foreach (Promocion promo in promocionesActuales)
        {
            GameObject obj = Instantiate(prefabOpcionPromo, contenedorPromociones);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(250, 300);
            rt.anchoredPosition = new Vector2(startX + (index * spacing), 0);
            index++;

            Transform imgProdT = obj.transform.Find("ImagenProducto");
            Transform imgPromoT = obj.transform.Find("ImagenPromo");

            Image imgProducto = imgProdT.GetComponent<Image>();
            Image imgPromo = imgPromoT.GetComponent<Image>();

            StartCoroutine(DownloadImage(promo.ImgProductoPromo, imgProducto));
            StartCoroutine(DownloadImage(promo.ImgPromo, imgPromo));

            Button btn = obj.GetComponent<Button>();
            int id = promo.IdPromocion;
            btn.onClick.AddListener(() => SeleccionarPromocion(id));
            botonesPromociones.Add(btn);
        }
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
    }

    IEnumerator TemporizadorCuentaRegresiva()
    {
        float tiempoRestante = tiempoLimite;
        int idUsuario = PlayerPrefs.GetInt("idUsuario", -1);

        while (tiempoRestante > 0 && !resultadoMostrado)
        {
            tiempoRestante -= Time.deltaTime;
            float porcentaje = tiempoRestante / tiempoLimite;

            if (barraTiempo != null)
            {
                barraTiempo.fillAmount = porcentaje;
                barraTiempo.color = porcentaje > 0.5f ? Color.green : (porcentaje > 0.2f ? Color.yellow : Color.red);
            }

            if (npcRenderer != null)
            {
                npcRenderer.color = Color.Lerp(Color.red, Color.white, porcentaje);
            }

            if (musicaFondo != null)
            {
                musicaFondo.pitch = Mathf.Lerp(2f, 1f, porcentaje);
            }

            yield return null;
        }

        if (!resultadoMostrado)
        {
            textoResultado.text = "¡Se acabó tu tiempo!";
            pantallaResultado.SetActive(true);
            resultadoMostrado = true;

            if (audioDerrota != null)
            {
                audioDerrota.Play();
            }

            StartCoroutine(FadeOutMusica());
        }
    }

    IEnumerator FadeOutMusica(float duracion = 2f)
    {
        if (musicaFondo == null) yield break;

        float tiempo = 0f;
        float pitchInicial = musicaFondo.pitch;
        float volumenInicial = musicaFondo.volume;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            musicaFondo.pitch = Mathf.Lerp(pitchInicial, 1f, t);
            musicaFondo.volume = Mathf.Lerp(volumenInicial, 0f, t);

            yield return null;
        }

        musicaFondo.pitch = 1f;
        musicaFondo.volume = 0f;
        musicaFondo.Stop();
    }

    public void AgregarBillete(float valor)
    {
        if (!billetesSeleccionados.ContainsKey(valor))
            billetesSeleccionados[valor] = false;

        billetesSeleccionados[valor] = !billetesSeleccionados[valor];
        cambioJugador += billetesSeleccionados[valor] ? valor : -valor;
        cambioJugador = Mathf.Max(0f, cambioJugador);
        textoCambioJugador.text = "$" + cambioJugador.ToString("0.00");
    }

    public void AgregarPeso()
    {
        cambioJugador += 1f;
        textoCambioJugador.text = "$" + cambioJugador.ToString("0.00");
    }

    public void QuitarPeso()
    {
        cambioJugador = Mathf.Max(0f, cambioJugador - 1f);
        textoCambioJugador.text = "$" + cambioJugador.ToString("0.00");
    }

    public void SeleccionarPromocion(int id)
    {
        idPromocionSeleccionada = id;
        for (int i = 0; i < botonesPromociones.Count; i++)
        {
            botonesPromociones[i].GetComponent<Image>().color =
                promocionesActuales[i].IdPromocion == id ? Color.green : Color.white;
        }
    }

    public void MostrarPromos() => contenedorPromos.SetActive(true);
    public void OcultarPromos() => contenedorPromos.SetActive(false);

    public void ConfirmarEntrega()
    {
        int idUsuario = PlayerPrefs.GetInt("idUsuario", -1);
        if (resultadoMostrado) return;

        bool cambioCorrecto = Mathf.Abs(cambioJugador - cambioEsperado) < 0.01f;
        bool promoCorrecta = idPromocionSeleccionada == idPromocionCorrecta;

        textoResultado.text = (cambioCorrecto ? "Cambio correcto\\n\\n" : "Cambio incorrecto\\n\\n") +
                              (promoCorrecta ? "Promoción correcta" : "Promoción incorrecta");

        pantallaResultado.SetActive(true);
        resultadoMostrado = true;

        if (cambioCorrecto && promoCorrecta)
        {
            textoResultado.text += "\n\n<color=green>+100 monedas</color> \\n <color=blue>+50 puntos</color>";
            if (audioVictoria != null)
                audioVictoria.Play();

            StartCoroutine(AgregarMonedasAJugador(idUsuario));
            StartCoroutine(SumarScoreAJugador(idUsuario, 50));
        }
        else if (audioDerrota != null)
        {
            audioDerrota.Play();
        }

        StartCoroutine(FadeOutMusica());
    }

    IEnumerator AgregarMonedasAJugador(int idUsuario)
    {
        string url = $"https://10.22.165.130:7275/api/GabrielMartinez/agregar-monedas/{idUsuario}";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "");
        request.certificateHandler = new ForceAcceptAll();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al agregar monedas: " + request.error);
        }
        else
        {
            Debug.Log("100 monedas agregadas correctamente.");
        }
    }
    IEnumerator SumarScoreAJugador(int idUsuario, int puntos)
    {
        string url = $"https://10.22.165.130:7275/api/GabrielMartinez/sumar-score/{idUsuario}?puntos={puntos}";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "");
        request.certificateHandler = new ForceAcceptAll();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al sumar score: " + request.error);
        }
        else
        {
            Debug.Log($"{puntos} puntos de score añadidos correctamente.");
        }
}


    public void IrAMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
